

import { Component, OnInit } from '@angular/core';
import { PatientService } from '../patient.service';
import { DoctorFilter } from '../../Shared/Models/doctor-filter.model';
import { Appointment } from '../../Shared/Models/appointment.model';


@Component({
  selector: 'app-view-doctors-list',
  templateUrl: './view-doctors-list.component.html',
  styleUrls: ['./view-doctors-list.component.scss']
})
export class ViewDoctorsListComponent implements OnInit {

  private readonly STORAGE_KEY = 'doctorSearchState';

  pageSizes = [
    { label: '4', value: 4 },
    { label: '8', value: 8 },
    { label: '12', value: 12 },
    { label: '20', value: 20 }
  ];

  today = new Date();
  doctors: any[] = [];
  genders: any[] = [];
  specializations: any[] = [];
  qualifications: any[] = [];
  isLoading = false;


  page = 1;
  pageSize = 4;
  totalRecords = 0;
  maxFeeLimit = 5000;
  bubblePosition = 0;

  filters: DoctorFilter = {
    Name: '',
    specializationIds: [],
    qualificationId: null,
    genderId: null,
    maxFee: null
  };

  constructor(private patientService: PatientService) { }

  ngOnInit(): void {
    this.loadFilterOptions();
    this.restoreState()
    this.fetchDoctors();
  }

  updateSliderBubble(event: Event): void {
    const input = event.target as HTMLInputElement;
    const value = +input.value;
    const min = +input.min || 0;
    const max = +input.max || 100;
    this.bubblePosition = ((value - min) / (max - min)) * 100;
  }

  private loadFilterOptions(): void {
    this.patientService.getAllFiltersDropdown().subscribe({
      next: (res) => {
        if (res.success && res.data) {
          this.genders = res.data.genders;
          this.specializations = res.data.specializations;
          this.qualifications = res.data.qualifications;
        }
      },
      error: (err) => console.error('Error fetching filters:', err)
    });
  }

  applyFilters(): void {
    this.page = 1;
    this.saveState();
    this.fetchDoctors();
  }


  fetchDoctors(): void {
    this.isLoading = true;
    debugger;
    const payload: Appointment = {
      DoctorID: 0,
      Name: this.filters.Name?.trim() || '',
      LastName: '',
      GenderID: this.filters.genderId || 0,
      QualificationIDs: this.filters.qualificationId || null,
      SpecializationIDs: this.filters.specializationIds.length
        ? this.filters.specializationIds.join(',')
        : null,
      MinFee: null,
      MaxFee: this.filters.maxFee || null,
      IsActive: true,
      PageNumber: this.page,
      PageSize: this.pageSize,
      SortColumn: 'FirstName',
      SortDirection: 'ASC'
    };

    this.patientService.getDoctorsList(payload).subscribe({
      next: (res) => {
        this.isLoading = false;

        if (res.success) {
          this.doctors = res.data || [];
          this.totalRecords = res.totalRecords || 0;
          if (this.totalRecords > 0) {
            const maxPage = Math.ceil(this.totalRecords / this.pageSize);
            if (this.page > maxPage) {
              this.page = maxPage;
            }
          }
          this.saveState();
        }
      },
      error: (err) => {
        this.isLoading = false;
        console.error('Error fetching doctors:', err);
      }
    });
  }


  onPageChange(newPage: number): void {
    this.page = newPage;
    this.saveState();
    this.fetchDoctors();
  }

  onPageSizeChange(newSize: number): void {
    this.pageSize = newSize;
    this.page = 1;
    this.saveState();
    this.fetchDoctors();
  }

  resetFilters(form: any): void {
    this.filters = {
      Name: '',
      specializationIds: [],
      qualificationId: null,
      genderId: null,
      maxFee: null
    };
    this.bubblePosition = 0;
    form?.resetForm(this.filters);

    this.page = 1;
    this.saveState();
    this.fetchDoctors();
  }

  private saveState(): void {
    const state = {
      filters: this.filters,
      page: this.page,
      pageSize: this.pageSize,
      bubblePosition: this.bubblePosition
    };
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(state));
  }

  private restoreState(): void {
    const saved = localStorage.getItem(this.STORAGE_KEY);
    if (saved) {
      try {
        const parsed = JSON.parse(saved);
        this.filters = parsed.filters || this.filters;
        debugger;
        this.page = parsed.page || this.page;
        this.pageSize = parsed.pageSize || this.pageSize;
        this.bubblePosition = parsed.bubblePosition || 0;
      } catch (e) {
        console.warn('Error parsing stored doctor filter state:', e);
        localStorage.removeItem(this.STORAGE_KEY);
      }
    }
  }

  goBack(): void {
    window.history.back();
  }

  getExperience(startDate: string | null): number {
    if (!startDate) return 0;
    return this.today.getFullYear() - new Date(startDate).getFullYear();
  }


  get startIndex(): number {
    return this.totalRecords ? (this.page - 1) * this.pageSize + 1 : 0;
  }

  get endIndex(): number {
    if (!this.totalRecords) return 0;
    const end = this.page * this.pageSize;
    return end > this.totalRecords ? this.totalRecords : end;
  }
}

