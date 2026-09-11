import { provideAppInitializer, inject } from '@angular/core';
import { NavItemsService } from '@abp/ng.theme.shared';
import { AboutDialogService } from './about-dialog.service';

function initAboutNavItem() {
  const navItems = inject(NavItemsService);
  const dialog = inject(AboutDialogService);

  navItems.addItems([
    {
      id: 'About',
      name: '::About',
      icon: 'fas fa-info-circle',
      order: 100,
      action: () => dialog.open(),
    },
  ]);
}

export const ABOUT_PROVIDER = [
  provideAppInitializer(() => {
    initAboutNavItem();
  }),
];
