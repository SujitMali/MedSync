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

  constructor(
    private adminService: AdminService,
    private toaster: ToasterService,
    private alert: AlertService
  ) { }

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


  isValidEmail(email: string): boolean {
    return /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9-]+\.[a-zA-Z]{2,}(?:\.[a-zA-Z]{2,})?$/.test(email?.trim() || '');
  }

  isValidPassword(password: string): boolean {
    return /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(password || '');
  }

  // ---------------- Form Submission ----------------
  submitCredentials(form: NgForm) {
    const { doctorId, email, password, roleId } = this.credentials;
    const errors: string[] = [];

    form.control.markAllAsTouched();

    if (!doctorId) errors.push('Please select a doctor.');
    if (!email) errors.push('Email is required.');
    else if (!this.isValidEmail(email)) errors.push('Please enter a valid email address.');

    if (!password) errors.push('Password is required.');
    else if (!this.isValidPassword(password))
      errors.push('Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.');

    if (!roleId) errors.push('Please select a role.');

    if (errors.length > 0) {
      const formattedErrors = `<ul style="text-align:left;">${errors.map(e => `<li>${e}</li>`).join('')}</ul>`;
      this.alert.warning(formattedErrors, 'Validation Errors');

      // Auto scroll to first invalid field
      const firstInvalid = document.querySelector('.ng-invalid');
      if (firstInvalid) {
        (firstInvalid as HTMLElement).scrollIntoView({ behavior: 'smooth', block: 'center' });
        (firstInvalid as HTMLElement).focus();
      }
      return;
    }

    // Everything valid — proceed
    const payload = {
      DoctorID: doctorId,
      Email: email,
      PasswordHash: password,
      RoleID: roleId
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
}
