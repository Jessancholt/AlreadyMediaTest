import { EnvironmentProviders, makeEnvironmentProviders } from '@angular/core';
import { MeteoritesApiService } from '../api';

export function providePages(): EnvironmentProviders {
  return makeEnvironmentProviders([
    MeteoritesApiService,
  ]);
}
