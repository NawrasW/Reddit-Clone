import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({
  name: 'timeAgo'
})
export class TimeAgoPipe implements PipeTransform {

  constructor(private datePipe: DatePipe) { }

  transform(value: string | undefined): string {
    if (!value) return ''; // Return empty string if value is undefined or null

    const date: Date = new Date(value);
    const now: Date = new Date();

    const diffMs: number = now.getTime() - date.getTime();
    const diffDays: number = Math.floor(diffMs / (1000 * 60 * 60 * 24));
    const diffHrs: number = Math.floor((diffMs % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const diffMins: number = Math.floor((diffMs % (1000 * 60 * 60)) / (1000 * 60));

    if (diffDays > 0) {
      return diffDays === 1 ? '1 day ago' : `${diffDays} days ago`;
    } else if (diffHrs > 0) {
      return diffHrs === 1 ? '1 hour ago' : `${diffHrs} hours ago`;
    } else {
      return diffMins === 1 ? '1 minute ago' : `${diffMins} minutes ago`;
    }
  }
}
