import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MeteoritesService } from '../services';
import { MatTableModule } from '@angular/material/table';
import { MatSortModule } from '@angular/material/sort';
import { SelectComponent } from 'src/app/shared/components/select/select.component';

export const displayColumns: readonly string[] = [
  'name',
  'recclass',
  'mass',
  'fall',
  'year',
  'reclat',
  'reclong',
];

@Component({
  selector: 'app-meteorites',
  templateUrl: './meteorites.component.html',
  styleUrls: ['./meteorites.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [MeteoritesService],
  imports: [SelectComponent, MatTableModule, MatSortModule],
})
export class MeteoritesComponent {
  private readonly service = inject(MeteoritesService);

  readonly classTitle = 'Class';
  readonly fromTitle = 'From';
  readonly toTitle = 'To';

  readonly context = this.service.context;

  readonly displayColumns = displayColumns;

  onFromChange($event: number | string): void {
    this.service.onFromChange($event as number);
  }

  onToChange($event: number | string): void {
    this.service.onToChange($event as number);
  }

  onClassChange($event: number | string): void {
    this.service.onClassChange($event as string);
  }
}
