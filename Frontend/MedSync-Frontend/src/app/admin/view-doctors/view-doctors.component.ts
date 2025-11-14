import { Component, OnInit } from '@angular/core';
import { AdminService } from '../admin.service';
import { Router } from '@angular/router';
import { ToasterService } from '../../Shared/Services/toaster.service';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-view-doctors',
  templateUrl: './view-doctors.component.html',
  styleUrls: ['./view-doctors.component.scss']
})
export class ViewDoctorsComponent implements OnInit {


  Math = Math;
  loading = true;
  doctors: any[] = [];
  totalRecords = 0;

  genders: any[] = [];
  bloodGroups: any[] = [];
  qualifications: any[] = [];
  specializations: any[] = [];


  filters = {
    name: '',
    genderID: null,
    bloodGroupID: null,
    qualificationIDs: [] as number[],
    specializationIDs: [] as number[],
    minFee: null as number | null,
    maxFee: null as number | null,
    PageNumber: 1,
    PageSize: 10,
    SortColumn: 'FirstName',
    SortDirection: 'ASC'
  };
  private feeChange$ = new Subject<void>();
  pageSizes = [
    { label: '5', value: 5 },
    { label: '10', value: 10 },
    { label: '20', value: 20 },
    { label: '50', value: 50 }
  ];

  constructor(private adminService: AdminService, private router: Router, private toaster: ToasterService) {
    this.feeChange$.pipe(debounceTime(2000)).subscribe(() => this.validateFees());
  }

  ngOnInit(): void {
    this.getDropdownData();
    this.loadDoctors();
  }

  Sorting(field: string): void {
    if (this.filters.SortColumn === field) {
      this.filters.SortDirection = this.filters.SortDirection === 'ASC' ? 'DESC' : 'ASC';
    } else {
      this.filters.SortColumn = field;
      this.filters.SortDirection = 'ASC';
    }
    this.loadDoctors();
  }

  getDropdownData(): void {
    this.adminService.getDropdownData().subscribe({
      next: (res: any) => {
        if (res.success) {
          this.genders = res.data.genders || [];
          this.bloodGroups = res.data.bloodGroups || [];
          this.qualifications = res.data.qualifications || [];
          this.specializations = res.data.specializations || [];
        }
      },
      error: (err) => this.toaster.error(err.error.message)
    });
  }

  loadDoctors(): void {
    this.loading = true;
    const formattedFilters = {
      Name: this.filters.name || null,
      GenderID: this.filters.genderID,
      BloodGroupID: this.filters.bloodGroupID,
      QualificationIDs: this.filters.qualificationIDs.length ? this.filters.qualificationIDs.join(',') : null,
      SpecializationIDs: this.filters.specializationIDs.length ? this.filters.specializationIDs.join(',') : null,
      MinFee: this.filters.minFee,
      MaxFee: this.filters.maxFee,
      PageNumber: this.filters.PageNumber,
      PageSize: this.filters.PageSize,


      SortColumn: this.filters.SortColumn,
      SortDirection: this.filters.SortDirection
    };

    this.adminService.getAllDoctorsList(formattedFilters).subscribe({
      next: (res: any) => {
        if (res.success) {
          this.doctors = res.data || [];
          this.totalRecords = res.totalRecords || 0;
        }
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        // alert('Error fetching doctors list.');
        this.toaster.error('Error fetching doctors list.')
      }
    });
  }

  applyFilters(): void {
    this.filters.PageNumber = 1;
    this.loadDoctors();
  }

  resetFilters(): void {
    this.filters = {
      name: '',
      genderID: null,
      bloodGroupID: null,
      qualificationIDs: [],
      specializationIDs: [],
      minFee: null,
      maxFee: null,
      PageNumber: 1,
      PageSize: 10,
      SortColumn: this.filters.SortColumn,
      SortDirection: this.filters.SortDirection
    };
    this.loadDoctors();
  }

  editDoctor(doc: any) {
    this.router.navigate(['/admin/edit-doctor', doc.DoctorID]);
  }
  feeWarning = '';

  onFeeChange() {
    this.feeChange$.next();
  }

  validateFees() {
    const { minFee, maxFee } = this.filters;

    if (minFee != null && maxFee != null) {
      if (minFee > maxFee) {
        this.feeWarning = 'Max fee adjusted to match minimum.';
        this.filters.maxFee = minFee;

      } else {
        this.feeWarning = '';
      }
    } else {
      this.feeWarning = '';
    }
  }

}

