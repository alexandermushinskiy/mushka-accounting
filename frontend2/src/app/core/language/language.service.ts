import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { languages } from '../../shared/constants/languages.const';

@Injectable()
export class LanguageService {
  private readonly defaultLanguage = languages.ru;
  readonly languages = [languages.ru];
  private currentLang$ = new BehaviorSubject<string>(this.defaultLanguage);

  constructor(private translateService: TranslateService) {
  }

  loadLanguageSettings() {
    this.translateService.addLangs(this.languages);
    this.translateService.setDefaultLang(this.defaultLanguage);

    this.loadCurrentLanguage();
  }

  useLanguage(lang: string) {
    this.translateService.use(lang);
    this.currentLang$.next(lang);
  }

  getCurrentLang(): Observable<string> {
    return this.currentLang$.asObservable();
  }

  translate(key: string, params?: object): string {
    return this.translateService.instant(key, params);
  }

  private loadCurrentLanguage() {
    const currentLang = this.defaultLanguage;

    this.currentLang$.next(currentLang);
    this.translateService.use(currentLang);
  }
}
