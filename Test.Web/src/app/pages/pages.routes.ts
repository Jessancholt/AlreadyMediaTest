import { Routes } from '@angular/router';
import { providePages } from './configuration';
import { MeteoritesComponent } from './sections/meteorites/components';

export const ROUTES: Routes = [
  {
    path: '',
    providers: [providePages()],
    children: [
      {
        path: '',
        redirectTo: 'meteorites',
        pathMatch: 'full',
      },
      {
        path: 'meteorites',
        component: MeteoritesComponent,
      },
    ],
  },
];
