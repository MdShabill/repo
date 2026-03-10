// export interface ServiceProvider {
//   id: number;
//   name: string;
// }

// export interface ServiceProviderResponse {
//   masterMasons: ServiceProvider[];
//   electricians: ServiceProvider[];
//   labours: ServiceProvider[];
//   plumbers: ServiceProvider[];
//   painters: ServiceProvider[];
//   carpenters: ServiceProvider[];
//   tilers: ServiceProvider[];
// }

// const SERVICE_API =
//   "https://localhost:7036/api/SiteAPI/service-providers";

// export const getServiceProviders =
//   async (): Promise<ServiceProviderResponse> => {
//     const res = await fetch(SERVICE_API);

//     if (!res.ok) throw new Error("Failed to load services");

//     return res.json();
//   };