import { HttpInterceptorFn, HttpResponseBase } from '@angular/common/http';
import { inject, Injector, runInInjectionContext } from '@angular/core';
import { catchError, Observable, of as just, switchMap, throwError } from 'rxjs';
import { HttpResponseCode, HttpResponseMessage } from './models';
import { ToastrService } from 'ngx-toastr';

export const errorInterceptor: HttpInterceptorFn = (request, next) => {
    const injector = inject(Injector);

    return next(request).pipe(
        catchError((err: HttpResponseBase) => {
            return getHttpResponseMessages(err).pipe(
                switchMap((messages) => {
                    runInInjectionContext(injector, () => {
                        processMessages(messages);
                    });
                    return throwError(() => err);
                }),
            );
        }),
    );
};

export function getHttpResponseMessages(
    response: HttpResponseBase | null,
): Observable<HttpResponseMessage | null> {
    if (!(response instanceof HttpResponseBase)) {
        return just(null);
    }

    switch (response.status) {
        case HttpResponseCode.notFound: {
            return just({
                code: response.status,
                title: 'Not found',
                message: 'Object was not found',
            });
        }
        case HttpResponseCode.serverError: {
            return just({
                code: response.status,
                title: 'Server error',
                message: 'Internal server error'
            });
        }
        case HttpResponseCode.badRequest: {
            return just({
                code: response.status,
                title: 'Bad request',
                message: 'Bad request to server'
            });
        }
        default: {
            return just({
                title: 'Unknown error',
                message: 'Unknown error was intercept'
            });
        }
    }
}

function processMessages(message: HttpResponseMessage | null): void {
    if (message == null) {
        return;
    }

    if (message.code != null) {
        inject(ToastrService).error(message.message, message.title);
    }
}