import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { AlertService } from '../../Shared/Services/alert.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-manage-credentials',
  templateUrl: './manage-credentials.component.html',
  styleUrls: ['./manage-credentials.component.scss']
})
export class ManageCredentialsComponent implements OnInit {
  doctorList: any[] = [];
  selectedDoctor: any = null;

  credentials = {
    doctorId: '',
    email: '',
    password: '',
    roleId: 2
  };

  roles = [
    { roleId: 1, roleName: 'Admin' },
    { roleId: 2, roleName: 'Doctor' }
  ];

  constructor(private adminService: AdminService, private toaster: ToasterService, private alert: AlertService) { }

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors() {
    this.adminService.getDoctorsWithoutUserCredentials().subscribe({
      next: (res: any) => {
        if (res?.success && res?.data) {
          this.doctorList = res.data.map((doc: any) => ({
            ...doc,
            fullName: `${doc.FirstName} ${doc.LastName}`
          }));
        } else {
          this.toaster.info('No doctors available to assign credentials.');
        }
        console.log(res.data);

      },
      error: () => this.toaster.error('Failed to load doctors list.')
    });
  }

  onDoctorChange(selectedDoctor: any) {
    this.selectedDoctor = selectedDoctor;
  }

  submitCredentials(form: NgForm) {
    form.control.markAllAsTouched();
    const errors = this.validateCredentials();

    if (errors.length > 0) {
      const formattedErrors = `<ul style="text-align:left;">${errors.map(e => `<li>${e}</li>`).join('')}</ul>`;
      this.alert.warning(formattedErrors, 'Validation Errors');
      return;
    }

    const payload = {
      DoctorID: this.credentials.doctorId,
      Email: this.credentials.email,
      PasswordHash: this.credentials.password,
      RoleID: this.credentials.roleId
    };

    this.adminService.addUserCredentials(payload).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.toaster.success(res.message);
          this.credentials = { doctorId: '', email: '', password: '', roleId: 2 };
          this.selectedDoctor = null;
          form.resetForm({ roleId: 2 });
          this.loadDoctors();
        } else {
          this.alert.error(res.message || 'Unable to add credentials.');
        }
      },
      error: (err) => this.alert.error(err.message || 'Server error occurred.')
    });
  }

  isValidEmail(email: string): boolean {
    return /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/.test(email?.trim() || '');
  }

  isValidPassword(password: string): boolean {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password || '');
  }

  private validateCredentials(): string[] {
    const { doctorId, email, password, roleId } = this.credentials;
    const errors: string[] = [];

    if (!doctorId) errors.push('Please select a doctor.');

    if (!email) errors.push('Email is required.');
    else if (!this.isValidEmail(email)) errors.push('Please enter a valid email address.');

    if (!password) errors.push('Password is required.');
    else if (!this.isValidPassword(password))
      errors.push('Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.');

    if (!roleId) errors.push('Please select a role.');

    return errors;
  }

}

