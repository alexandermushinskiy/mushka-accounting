import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SidebarMenuStateService {
  private isMenuCollapsed$ = new BehaviorSubject<boolean>(false);

  setCollapsedState(value: boolean) {
    this.isMenuCollapsed$.next(value);
  }

  isCollapsed(): Observable<boolean> {
    return this.isMenuCollapsed$.asObservable();
  }
}
