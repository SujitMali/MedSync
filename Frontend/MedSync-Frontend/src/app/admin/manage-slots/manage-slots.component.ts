import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';
import { DoctorSchedule } from '../../Shared/Models/doctor-schedule.model';
import { SlotDuration } from '../../Shared/Models/slot-duration.model';
import { CalendarOptions, EventInput } from '@fullcalendar/core';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { ToasterService } from '../../Shared/Services/toaster.service';

@Component({
  selector: 'app-manage-slots',
  templateUrl: './manage-slots.component.html',
  styleUrls: ['./manage-slots.component.scss']
})
export class ManageSlotsComponent implements OnInit {

  doctors: any[] = [];
  doctorSchedules: DoctorSchedule[] = [];
  slotDurations: SlotDuration[] = [];

  selectedDoctorID: number | null = null;
  selectedSlot: DoctorSchedule | null = null;
  calendarEvents: EventInput[] = [];

  calendarOptions!: CalendarOptions;

  constructor(private adminService: AdminService, private toaster: ToasterService) { }

  ngOnInit(): void {
    this.loadDoctors();
    this.loadSlotDurations();
    this.initializeCalendar();
  }

  loadDoctors(): void {
    const Filter: any = { PageSize: 100, IsActive: 1 };
    this.adminService.getAllDoctorsList(Filter).subscribe({
      next: (res: any) => {
        this.doctors = (res.data || []).map((d: any) => ({
          ...d,
          FullName: `${d.FirstName} ${d.LastName}`
        }));
      },
      error: () => this.toaster.error('Error loading doctors')
    });
  }

  loadSlotDurations(): void {
    this.adminService.getSlotDurations().subscribe(
      (res: any) => this.slotDurations = res.data || [],
      (err) => {
        console.error(err);
        this.slotDurations = [];
        this.toaster.error('Failed to load Slot Durations');
      }
    );
  }

  initializeCalendar(): void {
    this.calendarOptions = {
      plugins: [timeGridPlugin, interactionPlugin],
      initialView: 'timeGridWeek',
      editable: true,
      selectable: true,
      allDaySlot: false,
      slotMinTime: '06:00:00',
      slotMaxTime: '22:00:00',
      headerToolbar: false,
      dayHeaderFormat: { weekday: 'long' },
      events: this.calendarEvents,
      eventDrop: this.onEventChange.bind(this),
      eventResize: this.onEventChange.bind(this),
      eventClick: this.onEventClick.bind(this),
      select: this.onSelectTime.bind(this),
      eventOverlap: false,
      eventAllow: this.allowEventMove.bind(this)
    };
  }

  saveSchedules(): void {
    if (!this.selectedDoctorID) return;

    let payload = [...this.doctorSchedules];

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
        res.success
          ? this.toaster.success(res.message)
          : this.toaster.error(res.message);
        this.onDoctorSelect(this.selectedDoctorID!.toString());
      },
      (err) => this.toaster.error(err.error?.message ?? 'Save failed')
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



  // ===== Helpers Methods =======
  get selectedDoctor(): any {
    return this.doctors.find(d => d.DoctorID === this.selectedDoctorID);
  }

  private allowEventMove(dropInfo: any, draggedEvent: any): boolean {
    const start = dropInfo.start;
    const end = dropInfo.end;
    const dayOfWeek = start.getDay() + 1;
    const originalSlot: DoctorSchedule | undefined = draggedEvent?.extendedProps?.slot;

    const slotToTest: DoctorSchedule = {
      DoctorScheduleID: originalSlot?.DoctorScheduleID,
      DoctorID: this.selectedDoctorID ?? 0,
      DayOfWeek: dayOfWeek,
      StartTime: `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`,
      EndTime: `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`,
      SlotDurationID: originalSlot?.SlotDurationID ?? 0,
      IsActive: originalSlot?.IsActive ?? true
    };

    return !this.isOverlapping(slotToTest, originalSlot);
  }


  mapScheduleToEvent(schedule: DoctorSchedule): EventInput {
    const today = new Date();
    const firstDayOfWeek = new Date(today);
    firstDayOfWeek.setDate(today.getDate() - today.getDay());

    const eventDate = new Date(firstDayOfWeek);
    eventDate.setDate(firstDayOfWeek.getDate() + (schedule.DayOfWeek - 1));

    const [startH, startM] = schedule.StartTime.split(':').map(Number);
    const [endH, endM] = schedule.EndTime.split(':').map(Number);

    const start = new Date(eventDate);
    start.setHours(startH, startM, 0, 0);
    const end = new Date(eventDate);
    end.setHours(endH, endM, 0, 0);

    return {
      id: schedule.DoctorScheduleID
        ? schedule.DoctorScheduleID.toString()
        : `temp-${crypto.randomUUID()}`,
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

    if (this.isOverlapping(newSlot)) {
      this.toaster.warning('This slot overlaps with an existing schedule.');
      return;
    }

    this.doctorSchedules = [...this.doctorSchedules, newSlot];
    this.calendarEvents = [...this.calendarEvents, this.mapScheduleToEvent(newSlot)];
    this.refreshCalendar();
  }

  onEventChange(changeInfo: any): void {
    const slot: DoctorSchedule = changeInfo.event.extendedProps['slot'];
    const start: Date = changeInfo.event.start;
    const end: Date = changeInfo.event.end;

    const updatedSlot: DoctorSchedule = {
      ...slot,
      StartTime: `${start.getHours().toString().padStart(2, '0')}:${start.getMinutes().toString().padStart(2, '0')}`,
      EndTime: `${end.getHours().toString().padStart(2, '0')}:${end.getMinutes().toString().padStart(2, '0')}`,
      DayOfWeek: start.getDay() + 1
    };

    if (this.isOverlapping(updatedSlot, slot)) {
      this.toaster.error('This slot overlaps with another existing slot!');
      if (typeof changeInfo.revert === 'function') changeInfo.revert();
      return;
    }

    const idx = this.doctorSchedules.findIndex(s =>
      (s.DoctorScheduleID !== undefined && slot.DoctorScheduleID !== undefined && s.DoctorScheduleID === slot.DoctorScheduleID) ||
      (s.StartTime === slot.StartTime && s.EndTime === slot.EndTime && s.DayOfWeek === slot.DayOfWeek)
    );

    if (idx > -1) this.doctorSchedules[idx] = updatedSlot;

    changeInfo.event.setProp('title', this.getSlotTitle(updatedSlot));
    changeInfo.event.setExtendedProp('slot', updatedSlot);
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
    this.calendarOptions = {
      ...this.calendarOptions,
      events: [...this.calendarEvents]
    };
  }

  private isOverlapping(newSlot: DoctorSchedule, ignoreSlot?: DoctorSchedule | undefined): boolean {
    return this.doctorSchedules.some(existing => {
      if (existing === newSlot) return false;

      if (ignoreSlot) {
        if (existing.DoctorScheduleID !== undefined && ignoreSlot.DoctorScheduleID !== undefined &&
          existing.DoctorScheduleID === ignoreSlot.DoctorScheduleID) return false;
        if (existing.DayOfWeek === ignoreSlot.DayOfWeek &&
          existing.StartTime === ignoreSlot.StartTime &&
          existing.EndTime === ignoreSlot.EndTime) return false;
      }

      if (existing.DayOfWeek !== newSlot.DayOfWeek) return false;

      const [newStartH, newStartM] = newSlot.StartTime.split(':').map(Number);
      const [newEndH, newEndM] = newSlot.EndTime.split(':').map(Number);
      const [existStartH, existStartM] = existing.StartTime.split(':').map(Number);
      const [existEndH, existEndM] = existing.EndTime.split(':').map(Number);

      const newStart = newStartH * 60 + newStartM;
      const newEnd = newEndH * 60 + newEndM;
      const existStart = existStartH * 60 + existStartM;
      const existEnd = existEndH * 60 + existEndM;
      return newStart < existEnd && newEnd > existStart;
    });
  }
}

