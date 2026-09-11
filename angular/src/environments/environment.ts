import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44341/',
  redirectUri: baseUrl,
  clientId: 'Project_App',
  responseType: 'code',
  scope: 'offline_access Project',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'Project',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44341',
      rootNamespace: 'Abp.Dummy.Project',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
