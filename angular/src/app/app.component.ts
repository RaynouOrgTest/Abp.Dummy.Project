import { Component } from '@angular/core';
import { DynamicLayoutComponent } from '@abp/ng.core';
import { LoaderBarComponent } from '@abp/ng.theme.shared';
import { AboutComponent } from './about/about.component';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar />
    <abp-dynamic-layout />
    <app-about />
  `,
  imports: [LoaderBarComponent, DynamicLayoutComponent, AboutComponent],
})
export class AppComponent {}
