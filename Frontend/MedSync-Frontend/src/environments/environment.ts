const baseApi = 'https://localhost:44398/api';

export const environment = {
    production: false,
    apiUrl: baseApi,

    adminApiUrl: `${baseApi}/Admin`,
    doctorApiUrl: `${baseApi}/Doctor`,
    appointmentApiUrl: `${baseApi}/Appointment`,
    scheduleApiUrl: `${baseApi}/Schedule`
};
