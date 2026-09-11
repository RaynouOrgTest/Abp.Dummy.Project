import { Component, inject, OnInit, signal } from '@angular/core';
import { LocalizationPipe } from '@abp/ng.core';
import { ModalComponent, ModalCloseDirective } from '@abp/ng.theme.shared';
import { AboutService } from '../proxy/about';
import { AboutDialogService } from './about-dialog.service';

@Component({
  selector: 'app-about',
  imports: [ModalComponent, ModalCloseDirective, LocalizationPipe],
  template: `
    <abp-modal [(visible)]="dialog.visible">
      <ng-template #abpHeader>
        <h3>{{ '::About' | abpLocalization }}</h3>
      </ng-template>

      <ng-template #abpBody>
        <dl class="row mb-0">
          <dt class="col-6">{{ '::About:Version' | abpLocalization }}</dt>
          <dd class="col-6">{{ version() ?? '...' }}</dd>
        </dl>
      </ng-template>

      <ng-template #abpFooter>
        <button type="button" class="btn btn-secondary" abpClose>
          {{ '::Close' | abpLocalization }}
        </button>
      </ng-template>
    </abp-modal>
  `,
})
export class AboutComponent implements OnInit {
  protected dialog = inject(AboutDialogService);
  private aboutService = inject(AboutService);

  protected version = signal<string | null>(null);

  ngOnInit() {
    this.aboutService.getVersion().subscribe(result => {
      this.version.set(result.version);
    });
  }
}
