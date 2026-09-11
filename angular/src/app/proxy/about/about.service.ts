import type { ApplicationVersionDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AboutService {
  private restService = inject(RestService);
  apiName = 'Default';


  getVersion = (config?: Partial<Rest.Config>) =>
    this.restService.request<any, ApplicationVersionDto>({
      method: 'GET',
      url: '/api/app/about/version',
    },
    { apiName: this.apiName,...config });
}
