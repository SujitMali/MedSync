// import { Component, OnInit } from '@angular/core';
// import { AdminService } from '../admin.service';
// import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
// import { Doctor } from '../../Shared/Models/doctor.model';

// interface SlotDuration {
//   SlotDurationID: number;
//   DurationMinutes: number;
// }

// @Component({
//   selector: 'app-manage-slots',
//   templateUrl: './manage-slots.component.html',
//   styleUrls: ['./manage-slots.component.scss']
// })
// export class ManageSlotsComponent implements OnInit {

//   doctors: any[] = [];
//   selectedDoctorID: number | null = null;
//   doctorSchedules: DoctorSchedule[] = [];
//   slotDurations: SlotDuration[] = [];
//   daysOfWeek: string[] = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

//   isLoading: boolean = false;
//   saveMessage: string = '';

//   constructor(private adminService: AdminService) { }

//   ngOnInit(): void {
//     this.loadDoctors();
//     this.loadSlotDurations();
//   }

//   loadDoctors(): void {
//     this.isLoading = true;
//     this.adminService.getDoctorsList().subscribe(
//       (response: any) => {
//         this.doctors = response.data || [];
//         this.isLoading = false;
//       },
//       (err) => {
//         console.error(err);
//         this.doctors = [];
//         this.isLoading = false;
//       }
//     );
//   }

//   loadSlotDurations(): void {
//     this.adminService.getSlotDurations().subscribe(
//       (response: any) => {
//         this.slotDurations = response.data || [];
//       },
//       (err) => {
//         console.error(err);
//         this.slotDurations = [];
//       }
//     );
//   }

//   onDoctorSelect(doctorID: string): void {
//     this.selectedDoctorID = Number(doctorID);
//     this.doctorSchedules = [];
//     if (this.selectedDoctorID) {
//       this.isLoading = true;
//       this.adminService.getDoctorSchedule(this.selectedDoctorID).subscribe(
//         (data: any) => {
//           this.doctorSchedules = data.data || [];
//           this.isLoading = false;
//         },
//         (err) => {
//           console.error(err);
//           this.doctorSchedules = [];
//           this.isLoading = false;
//         }
//       );
//     }
//   }

//   saveSchedules(): void {
//     if (!this.selectedDoctorID || this.doctorSchedules.length === 0) return;

//     // Simple validation
//     for (const slot of this.doctorSchedules) {
//       if (!slot.StartTime || !slot.EndTime || !slot.SlotDurationID) {
//         this.saveMessage = 'Please fill all fields correctly before saving.';
//         return;
//       }
//     }

//     this.adminService.saveDoctorSchedule(this.doctorSchedules).subscribe(
//       (res: any) => {
//         this.saveMessage = res.success ? 'Schedules saved successfully!' : 'Failed to save schedules.';
//       },
//       (err) => {
//         console.error(err);
//         this.saveMessage = 'Error saving schedules.';
//       }
//     );
//   }

//   getDayName(dayIndex: number): string {
//     return this.daysOfWeek[dayIndex - 1] || '';
//   }
// }





// import { Component, OnInit } from '@angular/core';
// import { AdminService } from '../admin.service';
// import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
// import { Doctor } from '../../Shared/Models/doctor.model';
// import { CalendarOptions } from '@fullcalendar/core';
// import interactionPlugin from '@fullcalendar/interaction';
// import timeGridPlugin from '@fullcalendar/timegrid';



// interface SlotDuration {
//   SlotDurationID: number;
//   DurationMinutes: number;
// }

// @Component({
//   selector: 'app-manage-slots',
//   templateUrl: './manage-slots.component.html',
//   styleUrls: ['./manage-slots.component.scss']
// })
// export class ManageSlotsComponent implements OnInit {

//   doctors: any[] = [];
//   selectedDoctorID: number | null = null;
//   doctorSchedules: DoctorSchedule[] = [];
//   slotDurations: SlotDuration[] = [];
//   saveMessage: string = '';

//   calendarOptions: CalendarOptions = {
//     plugins: [timeGridPlugin, interactionPlugin],
//     initialView: 'timeGridWeek',
//     editable: true,
//     selectable: true,
//     allDaySlot: false,
//     slotMinTime: "06:00:00",
//     slotMaxTime: "22:00:00",
//     events: [],
//     eventDrop: this.onEventChange.bind(this),
//     eventResize: this.onEventChange.bind(this),
//     eventClick: this.onEventClick.bind(this)
//   };

//   constructor(private adminService: AdminService) { }

//   ngOnInit(): void {
//     this.loadDoctors();
//     this.loadSlotDurations();
//   }

//   loadDoctors(): void {
//     this.adminService.getDoctorsList().subscribe(
//       (response: any) => { this.doctors = response.data || []; },
//       (err) => { console.error(err); this.doctors = []; }
//     );
//   }

//   loadSlotDurations(): void {
//     this.adminService.getSlotDurations().subscribe(
//       (response: any) => { this.slotDurations = response.data || []; },
//       (err) => { console.error(err); this.slotDurations = []; }
//     );
//   }

//   onDoctorSelect(doctorID: string): void {
//     this.selectedDoctorID = Number(doctorID);
//     this.doctorSchedules = [];
//     this.calendarOptions.events = [];
//     if (this.selectedDoctorID) {
//       this.adminService.getDoctorSchedule(this.selectedDoctorID).subscribe(
//         (res: any) => {

//           this.doctorSchedules = res.data || [];
//           console.log(this.doctorSchedules);
//           this.calendarOptions.events = this.doctorSchedules
//             .filter(s => s.StartTime && s.EndTime)
//             .map(s => this.mapScheduleToEvent(s));

//         },
//         (err) => { console.error(err); this.doctorSchedules = []; }
//       );
//     }
//   }

//   getSlotTitle(slot: DoctorSchedule): string {
//     const duration = this.slotDurations.find(sd => sd.SlotDurationID === slot.SlotDurationID)?.DurationMinutes;
//     return `${duration || '-'} min ${slot.IsActive ? '' : '(Inactive)'}`;
//   }

//   onEventChange(changeInfo: any): void {
//     const slot: DoctorSchedule = changeInfo.event.extendedProps.slot;
//     slot.StartTime = changeInfo.event.start.toISOString();
//     slot.EndTime = changeInfo.event.end.toISOString();
//   }

//   onEventClick(clickInfo: any): void {
//     const slot: DoctorSchedule = clickInfo.event.extendedProps.slot;
//     // Optionally show a modal to edit SlotDuration or Active flag
//     const newDuration = prompt('Enter new duration in minutes:',
//       slot.SlotDurationID?.toString() || '');
//     if (newDuration) {
//       const sd = this.slotDurations.find(s => s.DurationMinutes === +newDuration);
//       if (sd) slot.SlotDurationID = sd.SlotDurationID;
//       clickInfo.event.title = this.getSlotTitle(slot);
//     }
//   }

//   saveSchedules(): void {
//     if (!this.selectedDoctorID || this.doctorSchedules.length === 0) return;

//     this.adminService.saveDoctorSchedule(this.doctorSchedules).subscribe(
//       (res: any) => { this.saveMessage = res.success ? 'Schedules saved successfully!' : 'Failed to save schedules.'; },
//       (err) => { console.error(err); this.saveMessage = 'Error saving schedules.'; }
//     );
//   }


//   mapScheduleToEvent(schedule: DoctorSchedule) {
//     const now = new Date();
//     const dayOfWeek = schedule.DayOfWeek % 7; // Sunday=0, Monday=1 ...

//     // start of this week (Sunday)
//     const sunday = new Date(now.setDate(now.getDate() - now.getDay()));

//     // add day offset
//     const eventDate = new Date(sunday);
//     eventDate.setDate(sunday.getDate() + dayOfWeek);

//     // parse HH:mm from schedule.StartTime
//     const [startH, startM] = schedule.StartTime.split(':').map(Number);
//     const [endH, endM] = schedule.EndTime.split(':').map(Number);

//     const start = new Date(eventDate);
//     start.setHours(startH, startM, 0, 0);

//     const end = new Date(eventDate);
//     end.setHours(endH, endM, 0, 0);

//     return {
//       id: schedule.DoctorScheduleID?.toString(),
//       title: this.getSlotTitle(schedule),
//       start,
//       end,
//       extendedProps: { slot: schedule }
//     };
//   }

// }









// import { Component, OnInit } from '@angular/core';
// import { AdminService } from '../admin.service';
// import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
// import { CalendarOptions } from '@fullcalendar/core';
// import interactionPlugin from '@fullcalendar/interaction';
// import timeGridPlugin from '@fullcalendar/timegrid';
// import { SlotDuration } from '../../Shared/Models/slot-duration.model';
// import { FullCalendarModule } from "@fullcalendar/angular";

// @Component({
//   selector: 'app-manage-slots',
//   templateUrl: './manage-slots.component.html',
//   styleUrls: ['./manage-slots.component.scss']
// })
// export class ManageSlotsComponent implements OnInit {

//   doctors: any[] = [];
//   selectedDoctorID: number | null = null;
//   doctorSchedules: DoctorSchedule[] = [];
//   slotDurations: SlotDuration[] = [];
//   saveMessage: string = '';

//   selectedSlot: DoctorSchedule | null = null;

//   calendarOptions: CalendarOptions = {
//     plugins: [timeGridPlugin, interactionPlugin],
//     initialView: 'timeGridWeek',
//     editable: true,
//     selectable: true,
//     allDaySlot: false,
//     slotMinTime: "06:00:00",
//     slotMaxTime: "22:00:00",
//     headerToolbar: false,
//     dayHeaderFormat: { weekday: 'long' },
//     events: [],
//     eventDrop: this.onEventChange.bind(this),
//     eventResize: this.onEventChange.bind(this),
//     eventClick: this.onEventClick.bind(this)
//   };

//   constructor(private adminService: AdminService) { }

//   ngOnInit(): void {
//     this.loadDoctors();
//     this.loadSlotDurations();
//   }

//   loadDoctors(): void {
//     this.adminService.getDoctorsList().subscribe(
//       (res: any) => { this.doctors = res.data || []; },
//       (err) => { console.error(err); this.doctors = []; }
//     );
//   }

//   loadSlotDurations(): void {
//     this.adminService.getSlotDurations().subscribe(
//       (res: any) => { this.slotDurations = res.data || []; },
//       (err) => { console.error(err); this.slotDurations = []; }
//     );
//   }

//   onDoctorSelect(doctorID: string): void {
//     this.selectedDoctorID = Number(doctorID);
//     this.doctorSchedules = [];
//     this.calendarOptions.events = [];
//     if (!this.selectedDoctorID) return;

//     this.adminService.getDoctorSchedule(this.selectedDoctorID).subscribe(
//       (res: any) => {
//         this.doctorSchedules = res.data || [];


//         this.calendarOptions.events = this.doctorSchedules
//           .filter(s => s.StartTime && s.EndTime)
//           .map(s => this.mapScheduleToEvent(s));
//       },
//       (err) => { console.error(err); this.doctorSchedules = []; }
//     );
//   }

//   mapScheduleToEvent(schedule: DoctorSchedule) {
//     const dayIndex = (schedule.DayOfWeek - 1) % 7; // 1=Sun → 0
//     const baseDate = new Date(1970, 0, 4); // Sunday
//     const eventDate = new Date(baseDate);
//     eventDate.setDate(baseDate.getDate() + dayIndex);

//     const [startH, startM] = schedule.StartTime.split(':').map(Number);
//     const [endH, endM] = schedule.EndTime.split(':').map(Number);

//     const start = new Date(eventDate);
//     start.setHours(startH, startM, 0, 0);

//     const end = new Date(eventDate);
//     end.setHours(endH, endM, 0, 0);

//     return {
//       id: schedule.DoctorScheduleID?.toString(),
//       title: this.getSlotTitle(schedule),
//       start,
//       end,
//       extendedProps: { slot: schedule }
//     };
//   }

//   getSlotTitle(slot: DoctorSchedule): string {
//     const duration = this.slotDurations.find(sd => sd.SlotDurationID === slot.SlotDurationID)?.DurationMinutes;
//     return `${duration || '-'} min ${slot.IsActive ? '' : '(Inactive)'}`;
//   }

//   onEventChange(changeInfo: any): void {
//     const slot: DoctorSchedule = changeInfo.event.extendedProps.slot;
//     const start = changeInfo.event.start;
//     const end = changeInfo.event.end;

//     slot.StartTime = `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`;
//     slot.EndTime = `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`;
//     slot.DayOfWeek = start.getDay() + 1;

//     changeInfo.event.setProp('title', this.getSlotTitle(slot));
//   }

//   onEventClick(clickInfo: any): void {
//     this.selectedSlot = clickInfo.event.extendedProps.slot;
//     const modal = document.getElementById('slotModal');
//     if (modal) modal.style.display = 'block';
//   }

//   selectSlotDuration(slotId: number): void {
//     if (this.selectedSlot) {
//       this.selectedSlot.SlotDurationID = slotId;

//       // const event = this.calendarOptions.events.find((e: any) => e.id === this.selectedSlot?.DoctorScheduleID?.toString());
//       const event = Array.isArray(this.calendarOptions.events)
//         ? this.calendarOptions.events.find((e: any) => e.id === this.selectedSlot?.DoctorScheduleID?.toString())
//         : null;

//       if (event) event.title = this.getSlotTitle(this.selectedSlot);

//       this.closeModal();
//     }
//   }

//   closeModal(): void {
//     const modal = document.getElementById('slotModal');
//     if (modal) modal.style.display = 'none';
//   }

//   saveSchedules(): void {
//     if (!this.selectedDoctorID || this.doctorSchedules.length === 0) return;

//     this.adminService.saveDoctorSchedule(this.doctorSchedules).subscribe(
//       (res: any) => { this.saveMessage = res.success ? 'Schedules saved successfully!' : 'Failed to save schedules.'; },
//       (err) => { console.error(err); this.saveMessage = 'Error saving schedules.'; }
//     );
//   }
// }




// ----------------------------------------------------------------------------------------------------------------------------------------


// import { Component, OnInit } from '@angular/core';
// import { AdminService } from '../admin.service';
// import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
// import { CalendarOptions } from '@fullcalendar/core';
// import interactionPlugin from '@fullcalendar/interaction';
// import timeGridPlugin from '@fullcalendar/timegrid';
// import { SlotDuration } from '../../Shared/Models/slot-duration.model';

// @Component({
//   selector: 'app-manage-slots',
//   templateUrl: './manage-slots.component.html',
//   styleUrls: ['./manage-slots.component.scss']
// })
// export class ManageSlotsComponent implements OnInit {

//   doctors: any[] = [];
//   selectedDoctorID: number | null = null;
//   doctorSchedules: DoctorSchedule[] = [];
//   slotDurations: SlotDuration[] = [];
//   saveMessage: string = '';

//   selectedSlot: DoctorSchedule | null = null;

//   calendarOptions: CalendarOptions = {
//     plugins: [timeGridPlugin, interactionPlugin],
//     initialView: 'timeGridWeek',
//     editable: true,
//     selectable: true,
//     allDaySlot: false,
//     slotMinTime: "06:00:00",
//     slotMaxTime: "22:00:00",
//     headerToolbar: false,
//     dayHeaderFormat: { weekday: 'long' },
//     events: [],
//     eventDrop: this.onEventChange.bind(this),
//     eventResize: this.onEventChange.bind(this),
//     eventClick: this.onEventClick.bind(this),
//     select: this.onSelectTime.bind(this)
//   };

//   constructor(private adminService: AdminService) { }

//   ngOnInit(): void {
//     this.loadDoctors();
//     this.loadSlotDurations();
//   }

//   loadDoctors(): void {
//     this.adminService.getDoctorsList().subscribe(
//       (res: any) => { this.doctors = res.data || []; },
//       (err) => { console.error(err); this.doctors = []; }
//     );
//   }

//   loadSlotDurations(): void {
//     this.adminService.getSlotDurations().subscribe(
//       (res: any) => { this.slotDurations = res.data || []; },
//       (err) => { console.error(err); this.slotDurations = []; }
//     );
//   }

//   onDoctorSelect(doctorID: string): void {
//     this.selectedDoctorID = Number(doctorID);
//     this.doctorSchedules = [];
//     (this.calendarOptions.events as any[]) = [];
//     if (!this.selectedDoctorID) return;

//     this.adminService.getDoctorSchedule(this.selectedDoctorID).subscribe(
//       (res: any) => {
//         this.doctorSchedules = res.data || [];
//         (this.calendarOptions.events as any[]) = this.doctorSchedules
//           .filter(s => s.StartTime && s.EndTime)
//           .map(s => this.mapScheduleToEvent(s));
//       },
//       (err) => { console.error(err); this.doctorSchedules = []; }
//     );
//   }

//   mapScheduleToEvent(schedule: DoctorSchedule) {
//     const today = new Date();
//     const firstDayOfWeek = new Date(today);
//     firstDayOfWeek.setDate(today.getDate() - today.getDay()); // Sunday of current week

//     const eventDate = new Date(firstDayOfWeek);
//     eventDate.setDate(firstDayOfWeek.getDate() + (schedule.DayOfWeek - 1));

//     const [startH, startM] = schedule.StartTime.split(':').map(Number);
//     const [endH, endM] = schedule.EndTime.split(':').map(Number);

//     const start = new Date(eventDate);
//     start.setHours(startH, startM, 0, 0);

//     const end = new Date(eventDate);
//     end.setHours(endH, endM, 0, 0);

//     return {
//       id: schedule.DoctorScheduleID?.toString(),
//       title: this.getSlotTitle(schedule),
//       start,
//       end,
//       extendedProps: { slot: schedule }
//     };
//   }

//   getSlotTitle(slot: DoctorSchedule): string {
//     const duration = this.slotDurations.find(sd => sd.SlotDurationID === slot.SlotDurationID)?.DurationMinutes;
//     return `${duration || '-'} min ${slot.IsActive ? '' : '(Inactive)'}`;
//   }

//   onEventChange(changeInfo: any): void {
//     const slot: DoctorSchedule = changeInfo.event.extendedProps.slot;
//     const start = changeInfo.event.start;
//     const end = changeInfo.event.end;

//     slot.StartTime = `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`;
//     slot.EndTime = `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`;
//     slot.DayOfWeek = start.getDay() + 1;

//     changeInfo.event.setProp('title', this.getSlotTitle(slot));
//   }

//   onEventClick(clickInfo: any): void {
//     this.selectedSlot = clickInfo.event.extendedProps.slot || null;
//     if (!this.selectedSlot) return;

//     const modal = document.getElementById('slotModal');
//     if (modal) modal.style.display = 'block';
//   }

//   onSelectTime(selection: any): void {
//     if (!this.selectedDoctorID) return;

//     const start = selection.start;
//     const end = selection.end;
//     const dayOfWeek = start.getDay() + 1;

//     const newSlot: DoctorSchedule = {
//       DoctorScheduleID: undefined,
//       DoctorID: this.selectedDoctorID,
//       DayOfWeek: dayOfWeek,
//       StartTime: `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`,
//       EndTime: `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`,
//       SlotDurationID: this.slotDurations[0]?.SlotDurationID ?? 0,
//       IsActive: true
//     };

//     this.doctorSchedules.push(newSlot);
//     (this.calendarOptions.events as any[]) = [
//       ...(this.calendarOptions.events as any[] || []),
//       this.mapScheduleToEvent(newSlot)
//     ];
//   }

//   selectSlotDuration(slotId: number): void {
//     if (!this.selectedSlot) return;

//     this.selectedSlot.SlotDurationID = slotId;

//     const event = (this.calendarOptions.events as any[]).find(
//       (e: any) => e.id === this.selectedSlot?.DoctorScheduleID?.toString()
//     );
//     if (event) event.title = this.getSlotTitle(this.selectedSlot);

//     this.closeModal();
//   }

//   removeSlot(slot: DoctorSchedule | null): void {
//     if (!slot) return;

//     this.doctorSchedules = this.doctorSchedules.filter(s => s !== slot);
//     (this.calendarOptions.events as any[]) = (this.calendarOptions.events as any[] || []).filter(
//       (e: any) => e.extendedProps.slot !== slot
//     );
//     this.closeModal();
//   }

//   closeModal(): void {
//     const modal = document.getElementById('slotModal');
//     if (modal) modal.style.display = 'none';
//   }

//   saveSchedules(): void {
//     if (!this.selectedDoctorID || this.doctorSchedules.length === 0) return;

//     this.adminService.saveDoctorSchedule(this.doctorSchedules).subscribe(
//       (res: any) => { this.saveMessage = res.success ? 'Schedules saved successfully!' : 'Failed to save schedules.'; },
//       (err) => { console.error(err); this.saveMessage = 'Error saving schedules.'; }
//     );
//   }

// }

//----------------------------------------------------------------------------------------------------------------------------


import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';
import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
import { SlotDuration } from '../../Shared/Models/slot-duration.model';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';

@Component({
  selector: 'app-manage-slots',
  templateUrl: './manage-slots.component.html',
  styleUrls: ['./manage-slots.component.scss']
})
export class ManageSlotsComponent implements OnInit {

  doctors: any[] = [];
  selectedDoctorID: number | null = null;
  doctorSchedules: DoctorSchedule[] = [];
  slotDurations: SlotDuration[] = [];
  saveMessage: string = '';

  selectedSlot: DoctorSchedule | null = null;
  calendarEvents: EventInput[] = [];

  calendarOptions: CalendarOptions = {
    plugins: [timeGridPlugin, interactionPlugin],
    initialView: 'timeGridWeek',
    editable: true,
    selectable: true,
    allDaySlot: false,
    slotMinTime: '06:00:00',
    slotMaxTime: '22:00:00',
    headerToolbar: false,
    dayHeaderFormat: { weekday: 'long' },
    events: [],
    eventDrop: this.onEventChange.bind(this),
    eventResize: this.onEventChange.bind(this),
    eventClick: this.onEventClick.bind(this),
    select: this.onSelectTime.bind(this)
  };

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.loadDoctors();
    this.loadSlotDurations();
  }

  // ====================== LOADERS ======================

  loadDoctors(): void {
    this.adminService.getDoctorsList().subscribe(
      (res: any) => this.doctors = res.data || [],
      (err) => { console.error(err); this.doctors = []; }
    );
  }

  loadSlotDurations(): void {
    this.adminService.getSlotDurations().subscribe(
      (res: any) => this.slotDurations = res.data || [],
      (err) => { console.error(err); this.slotDurations = []; }
    );
  }

  onDoctorSelect(doctorID: string): void {
    this.selectedDoctorID = Number(doctorID);
    this.doctorSchedules = [];
    this.calendarEvents = [];

    if (!this.selectedDoctorID) return;

    this.adminService.getDoctorSchedule(this.selectedDoctorID).subscribe(
      (res: any) => {
        this.doctorSchedules = res.data || [];
        this.calendarEvents = this.doctorSchedules
          .filter(s => s.StartTime && s.EndTime)
          .map(s => this.mapScheduleToEvent(s));
        this.refreshCalendar();
      },
      (err) => { console.error(err); this.doctorSchedules = []; }
    );
  }

  // ====================== MAPPING ======================

  mapScheduleToEvent(schedule: DoctorSchedule): EventInput {
    const today = new Date();
    const firstDayOfWeek = new Date(today);
    firstDayOfWeek.setDate(today.getDate() - today.getDay()); // Sunday as base

    const eventDate = new Date(firstDayOfWeek);
    eventDate.setDate(firstDayOfWeek.getDate() + (schedule.DayOfWeek - 1));

    const [startH, startM] = schedule.StartTime.split(':').map(Number);
    const [endH, endM] = schedule.EndTime.split(':').map(Number);

    const start = new Date(eventDate);
    start.setHours(startH, startM, 0, 0);
    const end = new Date(eventDate);
    end.setHours(endH, endM, 0, 0);

    return {
      // id: schedule.DoctorScheduleID?.toString() ?? crypto.randomUUID(),
      id: schedule.DoctorScheduleID
        ? schedule.DoctorScheduleID.toString()
        : `temp-${crypto.randomUUID()}`, // temporary ID for new slots

      title: this.getSlotTitle(schedule),
      start,
      end,
      extendedProps: { slot: schedule }
    };
  }

  getSlotTitle(slot: DoctorSchedule): string {
    const duration = this.slotDurations.find(sd => sd.SlotDurationID === slot.SlotDurationID)?.DurationMinutes;
    return `${duration || '-'} min ${slot.IsActive ? '' : '(Inactive)'}`;
  }

  // ====================== CALENDAR ACTIONS ======================

  onSelectTime(selection: any): void {
    if (!this.selectedDoctorID) return;

    const start = selection.start;
    const end = selection.end;
    const dayOfWeek = start.getDay() + 1;

    const newSlot: DoctorSchedule = {
      DoctorScheduleID: undefined,
      DoctorID: this.selectedDoctorID,
      DayOfWeek: dayOfWeek,
      StartTime: `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`,
      EndTime: `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`,
      SlotDurationID: this.slotDurations[0]?.SlotDurationID ?? 0,
      IsActive: true
    };

    this.doctorSchedules = [...this.doctorSchedules, newSlot];
    this.calendarEvents = [...this.calendarEvents, this.mapScheduleToEvent(newSlot)];
    this.refreshCalendar();
  }

  onEventChange(changeInfo: any): void {
    const slot: DoctorSchedule = changeInfo.event.extendedProps['slot'];
    const start = changeInfo.event.start;
    const end = changeInfo.event.end;

    slot.StartTime = `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`;
    slot.EndTime = `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`;
    slot.DayOfWeek = start.getDay() + 1;

    changeInfo.event.setProp('title', this.getSlotTitle(slot));

    // Preserve the schedule in the doctorSchedules array properly
    const idx = this.doctorSchedules.findIndex(s => s.DoctorScheduleID === slot.DoctorScheduleID);
    if (idx > -1) {
      this.doctorSchedules[idx] = slot; // update existing
    } else {
      // fallback: this is a new unsaved slot
      this.doctorSchedules.push(slot);
    }

  }

  onEventClick(clickInfo: any): void {
    this.selectedSlot = clickInfo.event.extendedProps['slot'] || null;
    const modal = document.getElementById('slotModal');
    if (modal) modal.style.display = 'block';
  }

  selectSlotDuration(slotId: number): void {
    if (!this.selectedSlot) return;
    this.selectedSlot.SlotDurationID = slotId;

    this.calendarEvents = this.calendarEvents.map(e =>
      e.extendedProps?.['slot'] === this.selectedSlot
        ? { ...e, title: this.getSlotTitle(this.selectedSlot!) }
        : e
    );
    this.refreshCalendar();
    this.closeModal();
  }

  removeSlot(slot: DoctorSchedule | null): void {
    if (!slot) return;
    this.doctorSchedules = this.doctorSchedules.filter(s => s !== slot);
    this.calendarEvents = this.calendarEvents.filter(e => e.extendedProps?.['slot'] !== slot);
    this.refreshCalendar();
    this.closeModal();
  }

  closeModal(): void {
    const modal = document.getElementById('slotModal');
    if (modal) modal.style.display = 'none';
  }

  refreshCalendar(): void {
    // replaces the reference to trigger change detection
    this.calendarOptions = {
      ...this.calendarOptions,
      events: [...this.calendarEvents]
    };
  }

  // ====================== SAVE ======================

  // saveSchedules(): void {
  //   if (!this.selectedDoctorID) return;

  //   this.adminService.saveDoctorSchedule(this.doctorSchedules).subscribe(
  //     (res: any) => {
  //       this.saveMessage = res.success ? 'Schedules saved successfully!' : 'Failed to save schedules.';
  //     },
  //     (err) => { console.error(err); this.saveMessage = 'Error saving schedules.'; }
  //   );
  // }

  saveSchedules(): void {
    if (!this.selectedDoctorID) return;

    let payload = [...this.doctorSchedules];

    // 👇 If all slots removed, send a dummy one just to carry DoctorID
    if (payload.length === 0) {
      payload = [{
        DoctorScheduleID: 0,
        DoctorID: this.selectedDoctorID,
        DayOfWeek: 0,
        StartTime: "00:00",
        EndTime: "00:00",
        SlotDurationID: 0,
        IsActive: false
      }];
    }

    this.adminService.saveDoctorSchedule(payload).subscribe(
      (res: any) => {
        this.saveMessage = res.success
          ? 'Schedules updated successfully!'
          : 'Failed to save schedules.';
      },
      (err) => {
        console.error(err);
        this.saveMessage = 'Error saving schedules.';
      }
    );
  }

}
