import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';

import { LogoutComponent } from './logout.component';
import { LogoutModule } from './logout.module';

const routerStub = {
  navigate: (commands: any[]) => { }
};

describe('LogoutComponent', () => {
  let component: LogoutComponent;
  let fixture: ComponentFixture<LogoutComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        LogoutModule
      ],
      providers: [
        {
          provide: Router,
          useValue: routerStub
        }
      ]
    });

    fixture = TestBed.createComponent(LogoutComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
