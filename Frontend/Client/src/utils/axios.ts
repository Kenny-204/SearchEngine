import axios from "axios";
// import showToast from "../components/toast/toast";
// import { redirect } from "next/navigation";
// import useAuthStore from "./store/auth";
// import useAdminAuthStore from "./store/admin-auth";

export const publicAxios = axios.create({
  baseURL: 'https://searchengine-yo6b.onrender.com/',
  timeout: 20000,
});

// // Add a response interceptor
// publicAxios.interceptors.response.use(function (response) {
//   // Any status code that lie within the range of 2xx cause this function to trigger
//   // Do something with response data
//   return response;
// }, function (error) {
//   if (error.response) {
//     showToast({message: error.response.data.message, type: "error"});
//   } else if (error.request) {
//     showToast({message: "Network error", type: "error"});
//   } else {
//     showToast({message: "Oops, something went wrong", type: "error"});
//   }
//   return Promise.reject(error);
// });

// export const privateAxios = axios.create({
//   ...publicAxios.defaults, // Inherit configuration from parent
//   headers: {
//     ...publicAxios.defaults.headers
//   },
// });

// // Add a request interceptor
// privateAxios.interceptors.request.use(function (config) {
//   // Do something before request is sent
//   const {accessToken} = useAuthStore.getState()
//   if (accessToken) {
//     config.headers.Authorization = `Bearer ${accessToken}`
//   }else {
//     // clearA
//   }
//   // if (config.method == "post") {
//   //   config.headers["Content-Type"] = "multipart/form-data"
//   // }
//   return config;
// }, function (error) {
//   // Do something with request error
//   return Promise.reject(error);
// });

// // Add a response interceptor
// privateAxios.interceptors.response.use(function (response) {
//   console.log(response.status)
//   return response;
// }, function (error) {
//   if (error.response) {
//     if (error.response.status == 401) {
//       const {clearAccessToken} = useAuthStore.getState()
//       clearAccessToken()
//     }
//     showToast({message: error.response.data.message, type: "error"});
//     console.log(error.response)
//   } else if (error.request) {
//     showToast({message: "Network error", type: "error"});
//   } else {
//     showToast({message: "Oops, something went wrong", type: "error"});
//   }
//   return Promise.reject(error);
// });

// export const adminAxios = axios.create({
//   ...publicAxios.defaults, // Inherit configuration from parent
//   headers: {
//     ...publicAxios.defaults.headers
//   },
//   baseURL: '/api/admin'
// });

// // Add a request interceptor
// adminAxios.interceptors.request.use(function (config) {
//   // Do something before request is sent
//   const {accessToken} = useAdminAuthStore.getState()
//   if (accessToken) {
//     config.headers.Authorization = `Bearer ${accessToken}`
//   }else {
//     // clearA
//   }
//   // if (config.method == "post") {
//   //   config.headers["Content-Type"] = "multipart/form-data"
//   // }
//   return config;
// }, function (error) {
//   // Do something with request error
//   return Promise.reject(error);
// });

// // Add a response interceptor
// adminAxios.interceptors.response.use(function (response) {
//   return response;
// }, function (error) {
//   if (error.response) {
//     if (error.response.status == 401) {
//       const {clearAccessToken} = useAdminAuthStore.getState()
//       clearAccessToken()
//     }
//     showToast({message: error.response.data.message, type: "error"});
//   } else if (error.request) {
//     showToast({message: "Network error", type: "error"});
//   } else {
//     showToast({message: "Oops, something went wrong", type: "error"});
//   }
//   return Promise.reject(error);
// });