import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AboutDialogService {
  visible = signal(false);

  open() {
    this.visible.set(true);
  }
}
