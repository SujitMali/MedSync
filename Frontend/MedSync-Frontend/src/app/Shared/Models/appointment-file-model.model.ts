    export interface AppointmentFileModel {
        AppointmentFileID: number;
        AppointmentID: number;
        FileName: string;
        FilePath: string;
        CreatedOn: string;
        CreatedBy?: number;
        ModifiedOn?: string;
        ModifiedBy?: number;
        IsActive: boolean;
    }