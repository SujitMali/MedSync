export interface Appointment {
    DoctorID: number;
    Name: string;
    LastName: string;
    GenderID: number;
    QualificationIDs: number | null;
    SpecializationIDs: string | null;
    MinFee: number | null;
    MaxFee: number | null;
    IsActive: boolean;
    PageNumber: number;
    PageSize: number;
    SortColumn: string;
    SortDirection: string;
}