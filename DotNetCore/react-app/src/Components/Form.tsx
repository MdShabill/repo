//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915807
// Building a Form

// import React from "react";

// const Form = () => {
//   return (
//     <form>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input id="name" type="text" className="form-control" />
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input id="age" type="number" className="form-control" />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915805
// Handling Form Submission

// import React, { FormEvent } from "react";

// const Form = () => {
//   const handleSubmit = (event: FormEvent) => {
//     event.preventDefault();
//     console.log("Submitted");
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input id="name" type="text" className="form-control" />
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input id="age" type="number" className="form-control" />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45997671
// Accessing Input Fields

// import React, { FormEvent, useRef } from "react";

// const Form = () => {
//   const nameRef = useRef<HTMLInputElement>(null);
//   const ageRef = useRef<HTMLInputElement>(null);
//   const person = { name: "", age: 0 };

//   const handleSubmit = (event: FormEvent) => {
//     event.preventDefault();

//     if (nameRef.current !== null) person.name = nameRef.current.value;
//     if (ageRef.current !== null) person.age = parseInt(ageRef.current.value);
//     console.log(person);
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input ref={nameRef} id="name" type="text" className="form-control" />
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input ref={ageRef} id="age" type="number" className="form-control" />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915808
// Controlled Components

// import React, { FormEvent, useState } from "react";

// const Form = () => {
//   const [person, setPerson] = useState({
//     name: "",
//     age: "",
//   });

//   const handleSubmit = (event: FormEvent) => {
//     event.preventDefault();
//     console.log(person);
//   };

//   return (
//     <form onSubmit={handleSubmit}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input
//           onChange={(event) =>
//             setPerson({ ...person, name: event.target.value })
//           }
//           value={person.name}
//           id="name"
//           type="text"
//           className="form-control"
//         />
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input
//           value={person.age}
//           onChange={(event) =>
//             setPerson({ ...person, age: event.target.value })
//           }
//           id="age"
//           type="number"
//           className="form-control"
//         />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915810
// Managing Forms With React Hook Form

// import { FormEvent, useState } from "react";
// import { FieldValues, useForm } from "react-hook-form";

// const Form = () => {
//   const { register, handleSubmit } = useForm();

//   const onSubmit = (data: FieldValues) => console.log(data);

//   return (
//     <form onSubmit={handleSubmit(onSubmit)}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input
//           {...register("name")}
//           id="name"
//           type="text"
//           className="form-control"
//         />
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input
//           {...register("age")}
//           id="age"
//           type="number"
//           className="form-control"
//         />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
//  https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915813
//  Appliying Validation

// import { FieldValues, useForm } from "react-hook-form";

// interface FormData {
//   name: string;
//   age: number;
// }

// const Form = () => {
//   const {
//     register,
//     handleSubmit,
//     formState: { errors },
//   } = useForm<FormData>();

//   const onSubmit = (data: FieldValues) => console.log(data);

//   return (
//     <form onSubmit={handleSubmit(onSubmit)}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input
//           {...register("name", { required: true, minLength: 3 })}
//           id="name"
//           type="text"
//           className="form-control"
//         />
//         {errors.name?.type === "requird" && (
//           <p className="text-danger">The name field is required.</p>
//         )}
//         {errors.name?.type === "minLength" && (
//           <p className="text-danger">The name must be at least 3 characters.</p>
//         )}
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input
//           {...register("age")}
//           id="age"
//           type="number"
//           className="form-control"
//         />
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915806
// Schema based validation with zod

// import { FieldValues, useForm } from "react-hook-form";
// import { z } from "zod";
// import { zodResolver } from "@hookform/resolvers/zod";

// const schema = z.object({
//   name: z.string().min(3, { message: "Name must be at least 3 characters." }),
//   age: z
//     .number({ invalid_type_error: "Age field is require." })
//     .min(18, { message: "Age must be at least 18." }),
// });

// type FormData = z.infer<typeof schema>;

// const Form = () => {
//   const {
//     register,
//     handleSubmit,
//     formState: { errors },
//   } = useForm<FormData>({ resolver: zodResolver(schema) });

//   const onSubmit = (data: FieldValues) => console.log(data);

//   return (
//     <form onSubmit={handleSubmit(onSubmit)}>
//       <div className="mb-3">
//         <label htmlFor="name" className="form-label">
//           <b>Name</b>
//         </label>
//         <input
//           {...register("name")}
//           id="name"
//           type="text"
//           className="form-control"
//         />
//         {errors.name && <p className="text-danger">{errors.name.message}</p>}
//       </div>

//       <div className="mb-3">
//         <label htmlFor="age" className="form-label">
//           <b>Age</b>
//         </label>
//         <input
//           {...register("age", { valueAsNumber: true })}
//           id="age"
//           type="number"
//           className="form-control"
//         />
//         {errors.age && <p className="text-danger">{errors.age.message}</p>}
//       </div>

//       <button className="btn btn-primary" type="submit">
//         <b>Submit</b>
//       </button>
//     </form>
//   );
// };

//------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915816
// Disabling the submit button

import { FieldValues, useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";

const schema = z.object({
  name: z.string().min(3, { message: "Name must be at least 3 characters." }),
  age: z
    .number({ invalid_type_error: "Age field is require." })
    .min(18, { message: "Age must be at least 18." }),
});

type FormData = z.infer<typeof schema>;

const Form = () => {
  const {
    register,
    handleSubmit,
    formState: { errors, isValid },
  } = useForm<FormData>({ resolver: zodResolver(schema) });

  const onSubmit = (data: FieldValues) => console.log(data);

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="mb-3">
        <label htmlFor="name" className="form-label">
          <b>Name</b>
        </label>
        <input
          {...register("name")}
          id="name"
          type="text"
          className="form-control"
        />
        {errors.name && <p className="text-danger">{errors.name.message}</p>}
      </div>

      <div className="mb-3">
        <label htmlFor="age" className="form-label">
          <b>Age</b>
        </label>
        <input
          {...register("age", { valueAsNumber: true })}
          id="age"
          type="number"
          className="form-control"
        />
        {errors.age && <p className="text-danger">{errors.age.message}</p>}
      </div>

      <button disabled={!isValid} className="btn btn-primary" type="submit">
        <b>Submit</b>
      </button>
    </form>
  );
};

export default Form;
