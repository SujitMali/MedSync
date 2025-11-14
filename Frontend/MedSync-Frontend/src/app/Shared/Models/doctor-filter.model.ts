export interface DoctorFilter {
    Name: string;
    specializationIds: number[];
    qualificationId: number | null;
    genderId: number | null;
    maxFee: number | null;
}
