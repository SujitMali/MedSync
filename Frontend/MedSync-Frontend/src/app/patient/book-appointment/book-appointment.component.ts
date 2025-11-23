import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PatientService } from '../patient.service';
import { AlertService } from '../../Shared/Services/alert.service';

declare var bootstrap: any;

@Component({
  selector: 'app-book-appointment',
  templateUrl: './book-appointment.component.html',
  styleUrls: ['./book-appointment.component.scss']
})
export class BookAppointmentComponent implements OnInit {

  today = formatDate(new Date(), 'yyyy-MM-dd', 'en');
  doctorId!: number;
  doctorDetails: any;
  selectedDate: string = formatDate(new Date(), 'yyyy-MM-dd', 'en');
  slots: any[] = [];
  selectedSlot: any = null;
  isLoadingSlots = false;
  patientModal: any;

  genders: any[] = [];
  bloodGroups: any[] = [];
  states: any[] = [];
  districts: any[] = [];
  talukas: any[] = [];
  filteredDistricts: any[] = [];
  filteredTalukas: any[] = [];

  selectedStateId: number | null = null;
  selectedDistrictId: number | null = null;
  todayDate = new Date();

  patient: any = {
    FirstName: '',
    LastName: '',
    GenderID: '',
    BloodGroupID: '',
    DateOfBirth: '',
    PhoneNumber: '',
    Email: '',
    Address: '',
    TalukaID: '',
    MedicalHistory: '',
    MedicalConcern: '',
    InsuranceDetails: ''
  };

  uploadedFiles: { file: File, customName: string }[] = [];

  constructor(
    private route: ActivatedRoute,
    private patientService: PatientService,
    private alertService: AlertService
  ) { }

  ngOnInit(): void {
    this.doctorId = +this.route.snapshot.paramMap.get('doctorId')!;
    this.loadDoctorDetails();
    this.loadDropdowns();
    this.fetchSlots();
  }

  loadDoctorDetails() {
    const payload = {
      DoctorID: this.doctorId,
      FirstName: '',
      LastName: '',
      GenderID: 0,
      QualificationIDs: null,
      SpecializationIDs: null,
      MinFee: null,
      MaxFee: null,
      IsActive: true,
      PageNumber: 1,
      PageSize: 1,
      SortColumn: 'FirstName',
      SortDirection: 'ASC'
    };

    this.patientService.getDoctorsList(payload).subscribe({
      next: (res) => {
        if (res.success && res.data.length > 0)
          this.doctorDetails = res.data[0];
      },
      error: (err) => console.error('Error fetching doctor details:', err)
    });
  }

  loadDropdowns() {
    this.patientService.getAllFiltersDropdown().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.genders = res.data.genders || [];
          this.bloodGroups = res.data.bloodGroups || [];
          this.states = res.data.states || [];
          this.districts = res.data.districts || [];
          this.talukas = res.data.talukas || [];
        }
      },
      error: (err) => console.error('Error loading dropdowns:', err)
    });
  }

  fetchSlots() {
    if (!this.selectedDate) return;
    this.isLoadingSlots = true;

    this.patientService.getDoctorSlots(this.doctorId, this.selectedDate).subscribe({
      next: (res) => {
        this.isLoadingSlots = false;
        this.slots = res.success ? res.data : [];
      },
      error: (err) => {
        this.isLoadingSlots = false;
        console.error('Error fetching slots:', err);
      }
    });
  }

  onStateChange() {
    const stateId = +(this.selectedStateId ?? 0);
    this.filteredDistricts = this.districts.filter(d => d.StateID === stateId);
    this.filteredTalukas = [];
    this.selectedDistrictId = null;
    this.patient.TalukaID = '';
  }

  onDistrictChange() {
    const districtId = +(this.selectedDistrictId ?? 0);
    this.filteredTalukas = this.talukas.filter(t => t.DistrictID === districtId);
    this.patient.TalukaID = '';
  }

  submitPatientForm(form?: any) {
    if (!this.selectedSlot) {
      this.alertService.warning('Please select a time slot before booking.', 'Slot Missing');
      return;
    }

    const isValid = this.validatePatientData(form);
    if (!isValid) return;

    const formData = new FormData();
    formData.append('DoctorID', this.doctorId.toString());
    formData.append('AppointmentDate', this.selectedDate);
    formData.append('StartTime', this.selectedSlot.SlotStart);
    formData.append('EndTime', this.selectedSlot.SlotEnd);
    formData.append('CreatedBy', '1');

    for (const key in this.patient) {
      if (this.patient[key] !== null && this.patient[key] !== undefined) {
        formData.append(`Patient_${key}`, this.patient[key]);
      }
    }

    this.uploadedFiles.forEach(f => {
      formData.append('Files', f.file, f.customName);
    });

    this.patientService.submitAppointmentWithFiles(formData).subscribe({
      next: (res) => {
        if (res.success) {
          this.alertService.success('Wait for Doctor to Accept Your Request', 'Appointment Requested!');
          this.patientModal.hide();
          this.selectedSlot = null;
          this.uploadedFiles = [];
          this.fetchSlots();
        } else {
          this.alertService.error('Failed to request appointment. Please try again later.', 'Request Failed');
        }
      },
      error: (err) => {
        console.error('Booking error:', err);
        this.alertService.error('An unexpected error occurred during booking.', 'Server Error');
      }
    });
  }




  //====== Helper Moethods ======
  selectSlot(slot: any) {
    if (!slot.IsAvailable) return;
    this.selectedSlot = slot;
  }

  confirmBooking() {
    if (!this.selectedSlot) return;
    const modalElement = document.getElementById('patientModal');
    this.patientModal = new bootstrap.Modal(modalElement, { backdrop: 'static', keyboard: false });
    this.patientModal.show();
  }

  onFilesSelected(event: any) {
    const files = Array.from(event.target.files) as File[];

    files.forEach(f => {
      this.uploadedFiles.push({ file: f, customName: f.name });
    });
  }

  private validatePatientData(form: any): boolean {
    // Angular form touch validation
    if (form && form.invalid) {
      Object.values(form.controls).forEach((control: any) => control.markAsTouched());
      this.alertService.error('Please fill all required patient details correctly before submitting.', 'Invalid Details');
      return false;
    }

    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const phonePattern = /^[0-9]{10}$/;

    // Email validation
    if (!emailPattern.test(this.patient.Email)) {
      this.alertService.error('Please enter a valid email address.', 'Invalid Email');
      return false;
    }

    // Phone validation
    if (!phonePattern.test(this.patient.PhoneNumber)) {
      this.alertService.error('Please enter a valid 10-digit phone number.', 'Invalid Phone');
      return false;
    }

    // String length validations (mirror server annotations)
    if (this.patient.FirstName && (this.patient.FirstName.length < 2 || this.patient.FirstName.length > 50)) {
      this.alertService.error('First Name must be between 2 and 50 characters.', 'Invalid Input');
      return false;
    }

    if (this.patient.LastName && (this.patient.LastName.length < 2 || this.patient.LastName.length > 50)) {
      this.alertService.error('Last Name must be between 2 and 50 characters.', 'Invalid Input');
      return false;
    }

    if (this.patient.Address && this.patient.Address.length > 200) {
      this.alertService.error('Address cannot exceed 200 characters.', 'Invalid Input');
      return false;
    }

    if (this.patient.MedicalHistory && this.patient.MedicalHistory.length > 300) {
      this.alertService.error('Medical History cannot exceed 300 characters.', 'Invalid Input');
      return false;
    }

    if (this.patient.MedicalConcern && this.patient.MedicalConcern.length > 300) {
      this.alertService.error('Medical Concern cannot exceed 300 characters.', 'Invalid Input');
      return false;
    }

    if (this.patient.InsuranceDetails && this.patient.InsuranceDetails.length > 200) {
      this.alertService.error('Insurance Details cannot exceed 200 characters.', 'Invalid Input');
      return false;
    }

    if (!this.patient.GenderID || this.patient.GenderID === '') {
      this.alertService.error('Please select a Gender.', 'Missing Field');
      return false;
    }

    if (!this.patient.TalukaID || this.patient.TalukaID === '') {
      this.alertService.error('Please select a Taluka.', 'Missing Field');
      return false;
    }

    return true;
  }

  goBack(): void {
    window.history.back();
  }

  getExperience(startDate: string | null): number {
    if (!startDate) return 0;
    return this.todayDate.getFullYear() - new Date(startDate).getFullYear();
  }

  removeFile(index: number) {
    this.uploadedFiles.splice(index, 1);
  }

  resetForm(patientForm: any) {
    patientForm.resetForm();
    this.uploadedFiles = [];
  }


  maxDate = formatDate(
    new Date(Date.now() + 14 * 24 * 60 * 60 * 1000),
    'yyyy-MM-dd',
    'en'
  );

}
