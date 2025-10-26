
import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../Shared/Models/doctor.model';
import { AdminService } from '../admin.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-doctors',
  templateUrl: './add-doctors.component.html',
  styleUrls: ['./add-doctors.component.scss']
})
export class AddDoctorsComponent implements OnInit {
  doctor: Doctor = new Doctor();

  genders: any[] = [];
  bloodGroups: any[] = [];
  qualifications: any[] = [];
  states: any[] = [];
  districts: any[] = [];
  talukas: any[] = [];
  filteredDistricts: any[] = [];
  filteredTalukas: any[] = [];

  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.doctor.profilePicOriginalName = file.name; // store original filename

      // Generate a preview for UI
      const reader = new FileReader();
      reader.onload = () => this.previewUrl = reader.result;
      reader.readAsDataURL(file);
    }
  }


  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadDropdownData();
  }

  loadDropdownData() {
    this.adminService.getDropdownData().subscribe({
      next: (response: any) => {
        if (response.success && response.data) {
          const data = response.data;

          this.genders = data.genders.map((g: any) => ({ id: g.GenderID, name: g.GenderName }));
          this.bloodGroups = data.bloodGroups.map((bg: any) => ({ id: bg.BloodGroupID, name: bg.BloodGroupName }));
          this.qualifications = data.qualifications.map((q: any) => ({ id: q.QualificationID, name: q.QualificationName }));
          this.states = data.states.map((s: any) => ({ id: s.StateID, name: s.StateName }));
          this.districts = data.districts.map((d: any) => ({ id: d.DistrictID, name: d.DistrictName, stateID: d.StateID }));
          this.talukas = data.talukas.map((t: any) => ({ id: t.TalukaID, name: t.TalukaName, districtID: t.DistrictID }));
        }
      },
      error: (err) => console.error('Error fetching dropdown data:', err)
    });
  }

  onStateChange(event: any) {
    const selectedStateId = Number(event.target.value);
    this.filteredDistricts = this.districts.filter(d => d.stateID === selectedStateId);
    this.filteredTalukas = []; // reset taluka list
  }

  onDistrictChange(event: any) {
    const selectedDistrictId = Number(event.target.value);
    this.filteredTalukas = this.talukas.filter(t => t.districtID === selectedDistrictId);
  }


  submitDoctor(form: NgForm) {
    if (form.valid) {
      const formData = new FormData();

      // Safely iterate using Object.entries
      Object.entries(this.doctor).forEach(([key, value]) => {
        if (value !== undefined && value !== null) {
          formData.append(key, value.toString());
        }
      });

      // Add profile picture if selected
      if (this.selectedFile) {
        formData.append('ProfilePicFile', this.selectedFile, this.selectedFile.name);
      }

      this.adminService.addDoctor(formData).subscribe({
        next: (res: any) => {
          if (res.success) {
            alert(`Doctor added successfully! ID: ${res.data.doctorID}`);
            form.resetForm();
            this.doctor = new Doctor();
            this.previewUrl = null;
            this.selectedFile = null;
          } else {
            alert('Failed: ' + res.message);
          }
        },
        error: (err) => {
          console.error('Error adding doctor:', err);
          alert('Error adding doctor. Check console for details.');
        }
      });
    } else {
      form.control.markAllAsTouched();
    }
  }


  // submitDoctor(form: NgForm) {
  //   if (form.valid) {
  //     console.log('Submitting doctor:', this.doctor);
  //     this.adminService.addDoctor(this.doctor).subscribe({
  //       next: (res: any) => {
  //         if (res.success) {
  //           alert(`Doctor added successfully! ID: ${res.doctorID}`);
  //           form.resetForm();
  //           this.doctor = new Doctor();
  //         } else alert('Failed: ' + res.message);
  //       },
  //       error: (err) => {
  //         console.error('Error adding doctor:', err);
  //         alert('Error adding doctor. Check console for details.');
  //       }
  //     });
  //   } else form.control.markAllAsTouched();
  // }

  resetForm(form: NgForm) {
    form.resetForm();
    this.filteredDistricts = [];
    this.filteredTalukas = [];
  }

}
