export interface MeteoritesContext {
  readonly data: readonly MeteoriteData[];
  readonly possibleYears: readonly number[];
  readonly possibleClasses: readonly string[];
}

export interface MeteoriteData {
  readonly id: string;
  readonly name: string;
  readonly class: string;
  readonly recclass: string;
  readonly mass: number;
  readonly fall: string;
  readonly year: number;
  readonly reclat: number;
  readonly reclong: number;
}
