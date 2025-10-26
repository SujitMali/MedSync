export class Doctor {
    doctorID?: number = 0;
    firstName: string = '';
    lastName: string = '';
    email: string = '';
    phoneNumber: string = '';
    genderID: number = 0;
    bloodGroupID: number = 0;
    qualificationID: number = 0;
    crriStartDate?: string | null = null;
    consultationFee: number = 0;
    dateOfBirth?: string | null = null;
    address?: string = '';
    talukaID?: number | null = null;
    isActive: boolean = true;
    createdOn?: string = new Date().toISOString();
    createdBy?: number | null = null;
    modifiedOn?: string | null = null;
    modifiedBy?: number | null = null;
    profilePicOriginalName?: string | null = null;
}
