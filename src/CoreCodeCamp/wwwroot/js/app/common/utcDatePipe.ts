import {Pipe, PipeTransform} from "@angular/core";
import * as moment from "moment/moment";

@Pipe({
  name: 'utcdate'
})
export class UtcDatePipe implements PipeTransform {
  transform(value: string, format: string): string {
    let momentDate = moment.utc(value);
    if (momentDate.isValid())
      return momentDate.format(format);
    else
      return value;
  }
}