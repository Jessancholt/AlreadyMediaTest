import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from '@angular/core';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatOption, MatSelect, MatSelectChange } from '@angular/material/select';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [MatFormField, MatSelect, MatLabel, MatOption],
})
export class SelectComponent {
  readonly title = input<string>();;
  readonly options = input<readonly string[] | readonly number[]>();

  readonly selectionChange = output<string | number>();

  onSelectionChange($event: MatSelectChange<string | number>): void {
    this.selectionChange.emit($event.value);
  }
}
