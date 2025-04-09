import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class WindowService {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  get nativeWindow(): Window | null {
    if (isPlatformBrowser(this.platformId)) {
      return window; // Return the window object if running in the browser
    }
    return null; // Return null if running on the server
  }
}