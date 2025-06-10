import { computed, inject, Injectable, signal } from '@angular/core';
import { rxResource, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { MeteoritesApiService } from 'src/app/pages/api';
import { QueryFilter } from '../models';
import { MeteoriteFilter } from 'src/app/pages/models';

@Injectable()
export class MeteoritesService {
  private readonly api = inject(MeteoritesApiService);
  private readonly router = inject(Router);

  constructor() {
    inject(ActivatedRoute)
      .queryParams.pipe(takeUntilDestroyed())
      .subscribe({
        next: (params) => {
          this.queryFilters.set(params);
        },
      });
  }

  private readonly queryFilters = signal<QueryFilter>({});

  private readonly _dataRx = rxResource({
    request: () => ({
      filter: this.queryFilters(),
    }),
    loader: ({ request: { filter } }) => {
      const mFilter: MeteoriteFilter = {
        name: filter.name,
        recclass: filter.recclass,
        from: filter.from,
        to: filter.to,
      };

      return this.api.getByFilter(mFilter);
    },
  });

  readonly context = computed(() => this._dataRx.value());

  onFromChange($event: number): void {
    this.applyQueryFilter({
      from: $event,
    });
  }

  onToChange($event: number): void {
    this.applyQueryFilter({
      to: $event,
    });
  }

  onClassChange($event: string): void {
    this.applyQueryFilter({
      recclass: $event,
    });
  }

  private applyQueryFilter(filter: QueryFilter): void {
    void this.router.navigate([], {
      queryParams: filter,
      queryParamsHandling: 'merge',
    });
  }
}
