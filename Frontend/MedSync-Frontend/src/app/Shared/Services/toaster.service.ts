import { Injectable } from '@angular/core';
import { Notyf } from 'notyf';

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  private notyf = new Notyf({
    duration: 3000,
    position: { x: 'center', y: 'top' },
    dismissible: true
  });

  private lastMessage: string | null = null;
  private lastShownTime = 0;
  private cooldown = 3000;

  private showToast(type: 'success' | 'error' | 'info' | 'warning', message: string, background?: string) {
    const now = Date.now();

    if (this.lastMessage === message && now - this.lastShownTime < this.cooldown) {
      return;
    }

    this.lastMessage = message;
    this.lastShownTime = now;

    switch (type) {
      case 'success':
        this.notyf.success(message);
        break;
      case 'error':
        this.notyf.error(message);
        break;
      case 'info':
        this.notyf.open({ type: 'info', message, background: background || '#2196f3' });
        break;
      case 'warning':
        this.notyf.open({ type: 'warning', message, background: background || '#ff9800' });
        break;
    }
  }

  success(message: string) {
    this.showToast('success', message);
  }

  error(message: string) {
    this.showToast('error', message);
  }

  info(message: string) {
    this.showToast('info', message, '#2196f3');
  }

  warning(message: string) {
    this.showToast('warning', message, '#ff9800');
  }
}
