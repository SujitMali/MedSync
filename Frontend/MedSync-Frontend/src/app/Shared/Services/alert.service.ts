import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  private baseOptions = {
    allowOutsideClick: false,
    allowEscapeKey: true,
    backdrop: true,
    showClass: {
      popup: 'swal2-show',
    },
    hideClass: {
      popup: 'swal2-hide',
    }
  };

  success(message: string, title: string = 'Success') {
    Swal.fire({
      ...this.baseOptions,
      title,
      html: message,
      icon: 'success',
      confirmButtonColor: '#3085d6',
      showConfirmButton: true
    });
  }

  error(message: string, title: string = 'Error') {
    Swal.fire({
      ...this.baseOptions,
      title,
      html: message,
      icon: 'error',
      confirmButtonColor: '#d33'
    });
  }

  warning(message: string, title: string = 'Warning') {
    Swal.fire({
      ...this.baseOptions,
      title,
      html: message,
      icon: 'warning',
      confirmButtonColor: '#f39c12'
    });
  }

  info(message: string, title: string = 'Info') {
    Swal.fire({
      ...this.baseOptions,
      title,
      html: message,
      icon: 'info',
      confirmButtonColor: '#3085d6'
    });
  }

  confirm(message: string, title: string = 'Are you sure?'): Promise<boolean> {
    return Swal.fire({
      ...this.baseOptions,
      title,
      text: message,
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#aaa',
      confirmButtonText: 'Yes',
      cancelButtonText: 'No'
    }).then(result => result.isConfirmed);
  }

  prompt(message: string, title: string = 'Input Required'): Promise<string | null> {
    return Swal.fire({
      title,
      text: message,
      input: 'text',
      inputPlaceholder: 'Enter here...',
      showCancelButton: true,
      confirmButtonText: 'Submit'
    }).then(result => result.isConfirmed ? result.value : null);
  }

}
