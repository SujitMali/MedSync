
import { Component } from '@angular/core';
import { AdminService } from '../admin.service';
import { ToasterService } from '../../Shared/Services/toaster.service';

declare const bootstrap: any;

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent {
  Math = Math;

  appointments: any[] = [];
  doctors: any[] = [];

  statuses: any[] = [];
  specializations: any[] = [];

  totalRecords = 0;
  loading = false;

  selectedAppointment: any = null;

  pageSizes = [
    { label: '5', value: 5 },
    { label: '10', value: 10 },
    { label: '20', value: 20 },
    { label: '50', value: 50 }
  ];

  filter: {
    DoctorID: number | null;
    StatusIDs: number[] | null;
    SpecializationIDs: number[] | null;
    DateFrom: string | null;
    DateTo: string | null;
    PatientName: string | null;
    PageNumber: number;
    PageSize: number;
    SortColumn: string;
    SortDirection: 'ASC' | 'DESC';
  } = {
      DoctorID: null,
      StatusIDs: [],
      SpecializationIDs: [],
      DateFrom: null,
      DateTo: null,
      PatientName: null,
      PageNumber: 1,
      PageSize: 10,
      SortColumn: 'AppointmentDate',
      SortDirection: 'ASC'
    };


  constructor(private adminService: AdminService, private toaster: ToasterService) { }


  ngOnInit(): void {
    this.loadDropDowns();
    this.loadAppointments();

  }

  loadDoctorsDropdown(): void {
    const Filter: any = {
      PageSize: 100,
      IsActive: 1
    }
    this.adminService.getAllDoctorsList(Filter).subscribe({
      next: (res: any) => {
        this.doctors = res.data || res.Data || [];
      },
      error: () => this.toaster.error('Error loading doctors')
    });
  }


  loadStatusesDropDown(): void {
    this.adminService.getAppointmentStatusesDropdown().subscribe({
      next: (res: any) => {
        this.statuses = res.data || [];
      },
      error: () => this.toaster.error('Error loading statuses')
    });
  }

  loadDropdownData(): void {
    this.adminService.getDropdownData().subscribe({
      next: (res: any) => {
        if (res.success && res.data) {
          this.specializations = res.data.specializations || [];
        }
      },
      error: () => this.toaster.error('Error loading dropdown data')
    });
  }

  loadDropDowns(): void {
    this.loadDoctorsDropdown();
    this.loadStatusesDropDown();
    this.loadDropdownData();
  }




  // loadAppointments(): void {
  //   this.loading = true;

  //   const payload = {
  //     ...this.filter,
  //     StatusIDs: Array.isArray(this.filter.StatusIDs)
  //       ? this.filter.StatusIDs.join(',')
  //       : this.filter.StatusIDs
  //   };

  //   this.adminService.getAllAppointments(payload).subscribe({
  //     next: (res: any) => {
  //       if (res.success) {
  //         this.appointments = res.data || [];
  //         this.totalRecords = res.totalRecords || 0;
  //         const files = res.files || [];
  //         this.appointments.forEach(app => {
  //           app.Files = files.filter((f: any) => f.AppointmentID === app.AppointmentID);
  //         });
  //       }
  //       this.loading = false;
  //     },
  //     error: () => (this.loading = false)
  //   });
  // }
  loadAppointments(): void {
    this.loading = true;

    const payload = {
      ...this.filter,
      StatusIDs: Array.isArray(this.filter.StatusIDs)
        ? this.filter.StatusIDs.join(',')
        : this.filter.StatusIDs,
      SpecializationIDs: Array.isArray(this.filter.SpecializationIDs)
        ? this.filter.SpecializationIDs.join(',')
        : this.filter.SpecializationIDs
    };

    this.adminService.getAllAppointments(payload).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.appointments = res.data || [];
          this.totalRecords = res.totalRecords || 0;
          const files = res.files || [];
          this.appointments.forEach(app => {
            app.Files = files.filter((f: any) => f.AppointmentID === app.AppointmentID);
          });
        }
        this.loading = false;
      },
      error: () => (this.loading = false)
    });
  }



  applyFilters(): void {
    this.filter.PageNumber = 1;
    this.loadAppointments();
  }

  resetFilters(): void {
    this.filter = {
      DoctorID: null,
      StatusIDs: null,
      SpecializationIDs: null,
      DateFrom: null,
      DateTo: null,
      PatientName: null,
      PageNumber: 1,
      PageSize: 10,
      SortColumn: 'AppointmentDate',
      SortDirection: 'ASC'
    };
    this.loadAppointments();
  }

  openDetails(app: any): void {
    this.selectedAppointment = app;
    const modalElement = document.getElementById('appointmentDetailsModal');
    if (modalElement) {
      const modal = new bootstrap.Modal(modalElement);
      modal.show();
    }
  }


  previewFile(file: any): void {
    if (!file || !file.FilePath || !this.selectedAppointment?.AppointmentID) {
      alert('File not found!');
      return;
    }

    const appointmentId = this.selectedAppointment.AppointmentID;

    this.adminService.previewAppointmentFile(appointmentId, file.FilePath).subscribe({
      next: (blob: Blob) => {
        const fileURL = URL.createObjectURL(blob);
        const previewWindow = window.open('', '_blank');
        if (previewWindow) {
          previewWindow.document.write(`
          <iframe src="${fileURL}" frameborder="0" style="width:100%;height:100vh;"></iframe>
        `);
        }
      },
      error: () => alert('Error previewing file!')
    });
  }
}

