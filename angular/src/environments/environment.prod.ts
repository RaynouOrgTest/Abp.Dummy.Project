import { Environment } from '@abp/ng.core';

export const environment = {
  production: true,
  application: {
    baseUrl: '',
    name: 'Project',
  },
  oAuthConfig: {
    issuer: '',
    redirectUri: '',
    clientId: 'Project_App',
    responseType: 'code',
    scope: 'offline_access Project',
    requireHttps: true,
  },
  apis: {
    default: {
      url: '',
      rootNamespace: 'Abp.Dummy.Project',
    },
    AbpAccountPublic: {
      url: '',
      rootNamespace: 'AbpAccountPublic',
    },
  },
  remoteEnv: {
    url: '/getEnvConfig',
    mergeStrategy: 'deepmerge',
  },
} as Environment;
