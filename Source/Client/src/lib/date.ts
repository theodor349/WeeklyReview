export function addDays(date: Date, days: number) {
  date.setDate(date.getDate() + days);
  return date;
}

export function roundMinutes(date: Date) {
  date.setMinutes(Math.floor(date.getMinutes() / 15) * 15);
  date.setSeconds(0);
  date.setMilliseconds(0);
}

export function getMinutes(date: Date) {
  return date.getHours() * 60 + date.getMinutes();
}