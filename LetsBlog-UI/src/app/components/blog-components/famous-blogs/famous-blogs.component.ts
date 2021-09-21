import { Component, OnInit } from '@angular/core';
import { Blog } from 'src/app/models/blog/blog.model';
import { BlogService } from 'src/app/services/blog.service';

@Component({
    selector: 'app-famous-blogs',
    templateUrl: './famous-blogs.component.html',
    styleUrls: ['./famous-blogs.component.scss'],
})
export class FamousBlogsComponent implements OnInit {
    famousBlogs: Blog[] = [];

    constructor(private blogService: BlogService) {}

    ngOnInit(): void {
        this.blogService.getMostFamous().subscribe((blogs) => {
            this.famousBlogs = blogs;
        });
    }
}
