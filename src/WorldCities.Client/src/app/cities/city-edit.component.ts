import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';

import { environment } from './../../environments/environment';
import { City } from './city';

@Component({
  selector: 'app-city-edit',
  templateUrl: './city-edit.component.html',
  styleUrls: ['./city-edit.component.scss'],
})
export class CityEditComponent implements OnInit {
  title?: string;
  form!: FormGroup;
  city?: City;

  constructor(
    private activatedRouter: ActivatedRoute,
    private router: Router,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.form = new FormGroup({
      name: new FormControl(''),
      lat: new FormControl(''),
      lon: new FormControl(''),
    });

    this.loadData();
  }

  loadData() {
    var idParam = this.activatedRouter.snapshot.paramMap.get('id');
    var id = idParam ? +idParam : 0;

    var url = environment.baseUrl + 'api/Cities/' + id;
    this.http.get<City>(url).subscribe(
      (result) => {
        this.city = result;
        this.title = 'Edit - ' + this.city.name;

        this.form.patchValue(this.city);
      },
      (error) => console.error(error)
    );
  }

  onSubmit() {
    var city = this.city;
    if (city) {
      city.name = this.form.controls['name'].value;
      city.name = this.form.controls['lat'].value;
      city.name = this.form.controls['lon'].value;

      var url = environment.baseUrl + 'api/Cities/' + city.id;
      this.http.put<City>(url, city).subscribe(
        (result) => {
          console.log('City ' + city!.id + ' has been updated.');
          this.router.navigate(['/cities']);
        },
        (error) => console.error(error)
      );
    }
  }
}
