import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../Shared/Models/doctor.model';
import { AdminService } from '../admin.service';
import { NgForm } from '@angular/forms';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { AlertService } from '../../Shared/Services/alert.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-doctors',
  templateUrl: './add-doctors.component.html',
  styleUrls: ['./add-doctors.component.scss']
})
export class AddDoctorsComponent implements OnInit {
  isSubmitting = false;
  isEditMode = false;

  doctor: Doctor = new Doctor();
  genders: any[] = [];
  bloodGroups: any[] = [];
  qualifications: any[] = [];
  specializations: any[] = [];
  states: any[] = [];
  districts: any[] = [];
  talukas: any[] = [];
  filteredDistricts: any[] = [];
  filteredTalukas: any[] = [];

  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;
  maxDate: string = '';
  minDate: string = '';

  isDOBValid: boolean = false;
  isAgeInvalid: boolean = false;
  isCRRIDateInvalid: boolean = false;


  constructor(private adminService: AdminService, private toaster: ToasterService, private alertService: AlertService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.formatDate();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.loadDropdownData(() => {
        this.loadDoctorById(+id);
      });
    } else {
      this.loadDropdownData();
    }
  }

  loadDropdownData(callback?: () => void): void {
    this.adminService.getDropdownData().subscribe({
      next: (response: any) => {
        if (response.success && response.data) {
          const data = response.data;

          this.genders = data.genders.map((g: any) => ({ id: g.GenderID, name: g.GenderName }));
          this.bloodGroups = data.bloodGroups.map((bg: any) => ({ id: bg.BloodGroupID, name: bg.BloodGroupName }));
          this.qualifications = data.qualifications.map((q: any) => ({ id: q.QualificationID, name: q.QualificationName }));
          this.specializations = data.specializations.map((s: any) => ({
            SpecializationID: s.SpecializationID,
            SpecializationName: s.SpecializationName
          }));

          this.states = data.states.map((s: any) => ({ id: s.StateID, name: s.StateName }));
          this.districts = data.districts.map((d: any) => ({ id: d.DistrictID, name: d.DistrictName, stateID: d.StateID }));
          this.talukas = data.talukas.map((t: any) => ({ id: t.TalukaID, name: t.TalukaName, districtID: t.DistrictID }));
        }
        if (callback) callback();
      },
      error: (err) => console.error('Error fetching dropdown data:', err)
    });
  }

  loadDoctorById(id: number): void {
    const body = { DoctorID: id };
    this.adminService.getAllDoctorsList(body).subscribe({

      next: (res: any) => {
        if (res.success && res.data && res.data.length > 0) {
          const d = res.data[0];
          this.doctor = d;
          this.doctor.SpecializationIDs = d.SpecializationIDs ? d.SpecializationIDs.split(',').map((id: string) => +id) : [];
          this.doctor.CRRIStartDate = d.CRRIStartDate ? d.CRRIStartDate.split('T')[0] : null;
          this.doctor.DateOfBirth = d.DateOfBirth ? d.DateOfBirth.split('T')[0] : null;
          if (this.doctor.TalukaID) {
            this.doctor.DistrictID = this.getDistrictByTaluka(this.doctor.TalukaID);
            this.doctor.StateID = this.getStateByTaluka(this.doctor.TalukaID);
          }
          this.onStateChange();
          this.onDistrictChange();
          this.previewUrl = d.ProfilePicPath || null;
        }

      },
      error: (err) => console.error('Error loading doctor:', err)
    });
  }

  onStateChange(): void {
    if (!this.doctor.StateID) {
      this.filteredDistricts = [];
      this.filteredTalukas = [];
      return;
    }
    this.filteredDistricts = this.districts.filter(d => d.stateID === this.doctor.StateID);
    this.filteredTalukas = [];
  }

  private getDistrictByTaluka(talukaID: number): number | null {
    const taluka = this.talukas.find(t => t.id === talukaID);
    return taluka ? taluka.districtID : null;
  }

  private getStateByTaluka(talukaID: number): number | null {
    const taluka = this.talukas.find(t => t.id === talukaID);
    const district = this.districts.find(d => d.id === taluka?.districtID);
    return district ? district.stateID : null;
  }

  onDistrictChange(): void {
    if (!this.doctor.DistrictID) {
      this.filteredTalukas = [];
      return;
    }
    this.filteredTalukas = this.talukas.filter(t => t.districtID === this.doctor.DistrictID);
  }

  submitDoctor(form: NgForm): void {

    this.validateDOB();
    this.validateCRRIStartDate();

    form.control.markAllAsTouched();

    const errors = this.validateDoctorModel();
    if (errors.length > 0) {
      const html = `<ul style="text-align:left;">${errors.map(e => `<li>${e}</li>`).join('')}</ul>`;
      this.alertService.warning(html, 'Please correct the following issues:');
      return;
    }

    this.alertService.confirm('Do you want to save this doctor record?', 'Confirm Submission')
      .then((confirmed) => {
        if (!confirmed) return;

        const formData = new FormData();
        Object.entries(this.doctor).forEach(([key, value]) => {
          if (value !== undefined && value !== null)
            formData.append(key, value.toString());
        });

        this.isSubmitting = true;
        this.adminService.addEditDoctor(formData).subscribe({
          next: (res: any) => {
            if (res.success) {
              this.toaster.success(res.message);
              this.alertService.success(res.message);
              this.router.navigate(['/admin/view-doctors']);
            } else {
              this.alertService.error(res.message);
            }
          },
          error: (err) => {
            this.alertService.error('Server error occurred.');
          },
          complete: () => (this.isSubmitting = false)
        });
      });
  }

  resetForm(form: NgForm): void {
    form.resetForm();
    this.filteredDistricts = [];
    this.filteredTalukas = [];
    this.previewUrl = null;
    this.selectedFile = null;
    this.isDOBValid = false;
  }

  private formatDate(): void {
    const today = new Date();
    today.setDate(today.getDate() - 1);
    this.maxDate = today.toISOString().split('T')[0];

    const minDOB = new Date(today);
    minDOB.setFullYear(minDOB.getFullYear() - 90);
    this.minDate = minDOB.toISOString().split('T')[0];
  }

  validateDOB(): void {
    if (!this.doctor.DateOfBirth) {
      this.isAgeInvalid = false;
      this.isDOBValid = false;
      return;
    }

    const dob = new Date(this.doctor.DateOfBirth);
    const today = new Date();

    if (dob >= today) {
      this.isAgeInvalid = true;
      this.isDOBValid = false;
      return;
    }
    const age = this.calculateAge(dob);
    const minAge = 23;
    const maxAge = 90;

    if (age < minAge || age > maxAge) {
      this.isAgeInvalid = true;
      this.isDOBValid = false;
      return;
    }

    this.isAgeInvalid = false;
    this.isDOBValid = true;
  }

  private calculateAge(dob: Date): number {
    const today = new Date();
    let age = today.getFullYear() - dob.getFullYear();
    const monthDiff = today.getMonth() - dob.getMonth();
    const dayDiff = today.getDate() - dob.getDate();
    if (monthDiff < 0 || (monthDiff === 0 && dayDiff < 0)) {
      age--;
    }
    return age;
  }

  validateCRRIStartDate(): void {
    this.isCRRIDateInvalid = false;
    if (!this.doctor.CRRIStartDate) return;
    const crriDate = new Date(this.doctor.CRRIStartDate);
    const today = new Date();
    if (crriDate >= today) {
      this.isCRRIDateInvalid = true;
      return;
    }
    if (this.doctor.DateOfBirth) {
      const dob = new Date(this.doctor.DateOfBirth);
      const minCRRIStart = new Date(dob);
      minCRRIStart.setFullYear(minCRRIStart.getFullYear() + 23);
      if (crriDate < minCRRIStart) {
        this.isCRRIDateInvalid = true;
        return;
      }
    }
  }

  private validateDoctorModel(): string[] {
    const d = this.doctor;
    const errors: string[] = [];

    // First Name
    if (!d.FirstName?.trim()) {
      errors.push('First Name is required.');
    } else if (!/^[A-Za-z][A-Za-z\s]*$/.test(d.FirstName)) {
      errors.push('First Name can only contain letters and spaces, starting with a letter.');
    } else if (d.FirstName.length < 2) {
      errors.push('First Name must be at least 2 characters long.');
    }

    // Last Name
    if (!d.LastName?.trim()) {
      errors.push('Last Name is required.');
    } else if (!/^[A-Za-z][A-Za-z\s]*$/.test(d.LastName)) {
      errors.push('Last Name can only contain letters and spaces, starting with a letter.');
    }

    // Phone Number
    if (!d.PhoneNumber) {
      errors.push('Phone Number is required.');
    } else if (!/^[0-9]{10}$/.test(d.PhoneNumber)) {
      errors.push('Phone Number must be exactly 10 digits.');
    }

    // DOB
    if (!d.DateOfBirth) {
      errors.push('Date of Birth is required.');
    } else if (this.isAgeInvalid) {
      errors.push('Doctor must be at least 23 years old.');
    }

    // Blood Group
    if (!d.BloodGroupID) errors.push('Blood Group is required.');

    // CRRI Date
    if (!this.isDOBValid) {
      errors.push('A valid Date of Birth is required before selecting CRRI Start Date.');
    } else if (!d.CRRIStartDate) {
      errors.push('CRRI Start Date is required.');
    } else if (this.isCRRIDateInvalid) {
      errors.push('CRRI Start Date must be before today and after valid age (minimum 23 years from DOB).');
    }

    // Gender
    if (!d.GenderID) errors.push('Gender is required.');

    // Consultation fee
    if (d.ConsultationFee === null || d.ConsultationFee === undefined) {
      errors.push('Consultation Fee is required.');
    } else if (isNaN(d.ConsultationFee)) {
      errors.push('Consultation Fee must be a valid number.');
    } else if (+d.ConsultationFee <= 0) {
      errors.push('Consultation Fee must be greater than 0.');
    }

    // Qualification
    if (!d.QualificationID) errors.push('Qualification is required.');

    // Specialization
    if (!d.SpecializationIDs || d.SpecializationIDs.length === 0) {
      errors.push('At least one specialization must be selected.');
    }

    // Address
    if (!d.Address?.trim()) {
      errors.push('Address is required.');
    }

    // Location
    if (!d.StateID) errors.push('State is required.');
    if (!d.DistrictID) errors.push('District is required.');
    if (!d.TalukaID) errors.push('Taluka is required.');

    return errors;
  }

}
