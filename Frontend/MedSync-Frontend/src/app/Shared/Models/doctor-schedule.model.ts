export class DoctorSchedule {
    DoctorScheduleID?: number = 0;
    DoctorID: number = 0;
    DayOfWeek: number = 0; // 1=Sunday ... 7=Saturday
    StartTime: string = '';
    EndTime: string = '';
    SlotDurationID: number = 0;
    IsActive: boolean = true;
    CreatedOn?: string = new Date().toISOString();
    CreatedBy?: number | null = null;
    ModifiedOn?: string | null = null;
    ModifiedBy?: number | null = null;
}
