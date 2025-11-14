export interface AppointmentFilterModel {
    doctorID?: number;
    statusIDs?: string;
    dateFrom?: string | null;
    dateTo?: string | null;
    patientName?: string | null;
    pageNumber?: number;
    pageSize?: number;
    sortColumn?: string;
    sortDirection?: string;
    status: string;
}
