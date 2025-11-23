import { Component, OnInit } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { CalendarOptions, EventClickArg } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import { AuthService } from '../../core/services/auth.service';

import { DoctorAppointmentsService } from '../doctor.service';
import { AlertService } from '../../Shared/Services/alert.service';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { AppointmentFilterModel } from '../../Shared/Models/appointment-filter.model';
import { DoctorAppointmentsViewModel } from '../../Shared/Models/doctor-appointments-view-model.model';
import { AppointmentModel } from '../../Shared/Models/appointment-model.model';

@Component({
  selector: 'app-view-appointment-request',
  templateUrl: './view-appointment-request.component.html',
  styleUrls: ['./view-appointment-request.component.scss'],
  animations: [
    trigger('fadeSlide', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('300ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ])
  ]
})
export class ViewAppointmentRequestComponent implements OnInit {

  doctorAppointments: DoctorAppointmentsViewModel = { appointments: [], appointmentFiles: [], totalRecords: 0 };
  filters: AppointmentFilterModel = {
    doctorID: 1,
    statusIDs: '2',
    dateFrom: '',
    dateTo: '',
    patientName: '',
    pageNumber: 1,
    pageSize: 5000,
    sortColumn: 'AppointmentDate',
    sortDirection: 'ASC',
    status: ''
  };

  statuses: any[] = [];
  currentView: 'calendar' | 'card' = 'calendar';
  loading = false;
  showModal = false;
  showDetailModal = false;
  showSuggestSlot = false;
  isLoadingSlots = false;
  selectedAppointment: AppointmentModel | null = null;
  selectedFiles: any[] = [];
  availableSlots: any[] = [];
  selectedSlot: any = null;
  suggestedDate: string = '';
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, interactionPlugin],
    initialView: 'dayGridMonth',
    events: [],
    displayEventTime: false,
    editable: false,
    selectable: true,
    eventClick: (info) => this.onEventClick(info)
  };
  
  today: string = new Date().toISOString().split('T')[0];

  constructor(private appointmentService: DoctorAppointmentsService, private authService: AuthService, private alertService: AlertService, private toaster: ToasterService) { }

  ngOnInit() {
    const user = this.authService.getUserFromStorage();
    if (user && user.doctorID) {
      this.filters.doctorID = user.doctorID;
    } else {
      console.warn('Doctor ID not found in user data');
    }
    this.filters = this.getDefaultFilters();
    this.loadStatuses();
    this.fetchAppointments();
  }

  loadStatuses() {
    this.appointmentService.getAppointmentStatuses().subscribe({
      next: (data) => {
        this.statuses = data.map(s => ({
          id: s.AppointmentStatusID,
          name: s.StatusName
        }));
      },
      error: (err) => console.error('Error loading statuses:', err)
    });
  }

  fetchAppointments() {
    this.loading = true;
    this.appointmentService.getDoctorAppointments(this.filters).subscribe({
      next: (data) => {
        debugger;
        this.doctorAppointments = data;
        console.log(this.doctorAppointments);

        this.selectedFiles = data.appointmentFiles;
        this.calendarOptions.events = data.appointments.map(appt => ({
          title: `${appt.PatientName} (${appt.StatusName})`,
          start: appt.AppointmentDate,
          backgroundColor: this.getStatusColor(appt.StatusName),
          extendedProps: { appointmentID: appt.AppointmentID }
        }));
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching appointments:', err);
        this.loading = false;
      }
    });
  }

  resetFilters() {
    this.filters = this.getDefaultFilters();
    this.fetchAppointments();
  }

  getDefaultFilters(): AppointmentFilterModel {
    const range = this.generateDateRange();
    return {
      doctorID: this.filters.doctorID || 1,
      statusIDs: '2',
      dateFrom: range.dateFrom,
      dateTo: range.dateTo,
      patientName: '',
      pageNumber: 1,
      pageSize: 5000,
      sortColumn: 'AppointmentDate',
      sortDirection: 'ASC',
      status: ''
    };
  }


  getStatusColor(status: string): string {
    const s = status.toLowerCase();
    if (s.includes('pending')) return 'brown';
    if (s.includes('accept')) return 'green';
    if (s.includes('cancel')) return 'red';
    if (s.includes('reject')) return 'violet';
    return '#6c757d';
  }

  onEventClick(info: EventClickArg) {
    const apptID = info.event.extendedProps['appointmentID'];
    const selected = this.doctorAppointments.appointments.find(a => a.AppointmentID === apptID);
    if (selected) {
      this.selectedAppointment = selected;
      this.selectedFiles = this.doctorAppointments.appointmentFiles.filter(f => f.AppointmentID === apptID);
      this.showDetailModal = true;
    }
  }

  closeDetailModal() {
    this.showDetailModal = false;
    this.selectedAppointment = null;
    this.selectedFiles = [];
    this.showSuggestSlot = false;
    this.availableSlots = [];
    this.selectedSlot = null;
  }

  acceptAppointment(appt: AppointmentModel) {
    const status = this.statuses.find(s => s.name === 'Accepted');
    if (!status) {
      this.toaster.error('Accepted status not found.');
      return;
    }

    this.alertService.confirm('Do you want to accept this appointment?', 'Confirm Acceptance')
      .then((confirmed) => {
        if (!confirmed) return;

        const payload = {
          appointmentID: appt.AppointmentID,
          doctorID: appt.DoctorID,
          appointmentStatusID: status.id
        };

        this.appointmentService.updateDoctorAppointment(payload).subscribe({
          next: () => {
            this.toaster.success('Appointment accepted successfully!');
            this.fetchAppointments();
            this.closeDetailModal();
          },
          error: () => {
            this.toaster.error('Error accepting appointment.');
          }
        });
      });
  }

  rejectAppointment(appt: AppointmentModel) {
    const status = this.statuses.find(s => s.name === 'Rejected');
    if (!status) {
      this.toaster.error('Rejected status not found.');
      return;
    }

    this.alertService.confirm('Do you want to reject this appointment?', 'Confirm Rejection')
      .then((confirmed) => {
        if (!confirmed) return;
        this.alertService.prompt('Please enter a reason for rejection:', 'Rejection Reason')
          .then((reason) => {
            if (!reason) {
              this.toaster.warning('Rejection reason is required.');
              return;
            }
            debugger;
            const payload = {
              appointmentID: appt.AppointmentID,
              doctorID: appt.DoctorID,
              appointmentStatusID: status.id,
              cancellationReason: reason
            };

            this.appointmentService.updateDoctorAppointment(payload).subscribe({
              next: () => {
                this.toaster.success('Appointment rejected successfully!');
                this.fetchAppointments();
                this.closeDetailModal();
              },
              error: () => {
                this.toaster.error('Error rejecting appointment.');
              }
            });
          });
      });
  }


  toggleSuggestSlot() {
    this.showSuggestSlot = !this.showSuggestSlot;
  }

  onDateChangeForSuggestion() {
    if (!this.suggestedDate || !this.selectedAppointment) return;

    this.isLoadingSlots = true;
    this.availableSlots = [];
    this.selectedSlot = null;

    this.appointmentService
      .getDoctorSlots(this.selectedAppointment.DoctorID, this.suggestedDate)
      .subscribe({
        next: (res) => {
          this.isLoadingSlots = false;
          this.availableSlots = res.success ? res.data : res;
        },
        error: (err) => {
          this.isLoadingSlots = false;
          console.error('Error loading slots:', err);
        }
      });
  }

  selectSlot(slot: any) {
    if (!slot.IsAvailable) return;
    this.selectedSlot = slot;
  }


  submitSuggestedSlot() {
    if (!this.selectedAppointment) return;
    if (!this.selectedSlot) return this.toaster.warning('Please select a slot.');

    const appt = this.selectedAppointment;

    this.alertService.confirm('Do you want to suggest this slot to the patient?', 'Confirm Suggestion')
      .then((confirmed) => {
        if (!confirmed) return;

        const startDate = new Date(this.selectedSlot.SlotStart);
        const endDate = new Date(this.selectedSlot.SlotEnd);

        const startTimeString = startDate.toTimeString().slice(0, 8);
        const endTimeString = endDate.toTimeString().slice(0, 8);

        const payload = {
          appointmentID: appt.AppointmentID,
          doctorID: appt.DoctorID,
          appointmentStatusID: this.statuses.find(s => s.name === 'Pending Patient Confirmation')?.id,
          appointmentDate: this.suggestedDate,
          startTime: startTimeString,
          endTime: endTimeString,
          isDoctorSuggestedChange: true
        };

        this.appointmentService.updateDoctorAppointment(payload).subscribe({
          next: () => {
            this.toaster.success('Suggested slot successfully sent to patient.');
            this.fetchAppointments();
            this.closeDetailModal();
          },
          error: () => this.toaster.error('Error suggesting slot.'),
        });
      });
  }


  openModal() {
    this.showModal = true;
    document.body.style.overflow = 'hidden';
  }

  closeModal() {
    this.showModal = false;
    document.body.style.overflow = 'auto';
  }


  openFile(file: any) {
    console.log(file);
    if (!file.FilePath) return;
    this.appointmentService.previewAppointmentFile(file.AppointmentID, file.FilePath).subscribe({
      next: (blob: Blob) => {
        const fileURL = URL.createObjectURL(blob);
        const previewWindow = window.open('', '_blank');
        if (previewWindow) {
          previewWindow.document.write(`
          <iframe src="${fileURL}" frameborder="0" style="width:100%;height:100vh;"></iframe>
        `);
        }
      },
      error: (err) => {
        console.error('Error previewing file:', err);
      }
    });
  }


  canModify(appt: AppointmentModel | null): boolean {
    if (!appt || !appt.StatusName) return false;
    return appt.StatusName.toLowerCase() === 'pending';
  }


  //====== Helper Methods =======
  calculateAge(dob: string | null): number | null {
    if (!dob) return null;
    const birth = new Date(dob);
    const today = new Date();
    let age = today.getFullYear() - birth.getFullYear();
    const m = today.getMonth() - birth.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) {
      age--;
    }
    console.log(age);

    return age;
  }

  generateDateRange() {
    const today = new Date();
    const dateTo = new Date(today);
    dateTo.setDate(today.getDate() + 30);

    const toStr = (d: Date) => d.toISOString().split('T')[0];

    return {
      dateFrom: toStr(today),
      dateTo: toStr(dateTo)
    };
  }

}

