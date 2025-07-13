//----------------------------------------
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915809
//building the Expense form

// import React from "react";
// import { categories } from "../../App";

// const ExpenseForm = () => {
//   return (
//     <form>
//       <div className="mb-3">
//         <label htmlFor="description" className="form-label">
//           Description
//         </label>
//         <input id="description" type="text" className="form-control" />
//       </div>
//       <br />
//       <div className="mb-3">
//         <label htmlFor="amount" className="form-label">
//           Amount
//         </label>
//         <input id="amount" type="number" className="form-control" />
//       </div>
//       <br />
//       <div className="mb-3">
//         <label htmlFor="category" className="form-label">
//           Category
//         </label>
//         <select id="category" className="form-select">
//           <option value=""></option>
//           {categories.map((category) => (
//             <option key={category} value={category}>
//               {category}
//             </option>
//           ))}
//         </select>
//       </div>
//       <button className="btn btn-primary">Submit</button>
//     </form>
//   );
// };

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915814
// Integrating with react hook form and zod

// import { z } from "zod";
// import { useForm } from "react-hook-form";
// import { zodResolver } from "@hookform/resolvers/zod";
// import categories from "../categories";

// const schema = z.object({
//   descripion: z
//     .string()
//     .min(3, { message: "Description Should be at least 3 characters." })
//     .max(50),
//   amount: z
//     .number({ invalid_type_error: "Amount is required." })
//     .min(0.01)
//     .max(100_000),
//   category: z.enum(categories, {
//     errorMap: () => ({ message: "Category is required." }),
//   }),
// });

// type ExpenseFormData = z.infer<typeof schema>;

// const ExpenseForm = () => {
//   const {
//     register,
//     handleSubmit,
//     formState: { errors },
//   } = useForm<ExpenseFormData>({ resolver: zodResolver(schema) });
//   return (
//     <form onSubmit={handleSubmit((data) => console.log(data))}>
//       <div className="mb-3">
//         <label htmlFor="description" className="form-label">
//           Description
//         </label>
//         <input
//           {...register("descripion")}
//           id="description"
//           type="text"
//           className="form-control"
//         />
//         {errors.descripion && (
//           <p className="text-danger">{errors.descripion.message}</p>
//         )}
//       </div>
//       <br />
//       <div className="mb-3">
//         <label htmlFor="amount" className="form-label">
//           Amount
//         </label>
//         <input
//           {...register("amount", { valueAsNumber: true })}
//           id="amount"
//           type="number"
//           className="form-control"
//         />
//         {errors.amount && (
//           <p className="text-danger">{errors.amount.message}</p>
//         )}
//       </div>
//       <br />
//       <div className="mb-3">
//         <label htmlFor="category" className="form-label">
//           Category
//         </label>
//         <select {...register("category")} id="category" className="form-select">
//           <option value=""></option>
//           {categories.map((category) => (
//             <option key={category} value={category}>
//               {category}
//             </option>
//           ))}
//         </select>
//         {errors.category && (
//           <p className="text-danger">{errors.category.message}</p>
//         )}
//       </div>
//       <button className="btn btn-primary">Submit</button>
//     </form>
//   );
// };

//----------------------------------------
// https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45917480
// Adding an expense

import { z } from "zod";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import categories from "../categories";

const schema = z.object({
  description: z
    .string()
    .min(3, { message: "Description Should be at least 3 characters." })
    .max(50),
  amount: z
    .number({ invalid_type_error: "Amount is required." })
    .min(0.01)
    .max(100_000),
  category: z.enum(categories, {
    errorMap: () => ({ message: "Category is required." }),
  }),
});

type ExpenseFormData = z.infer<typeof schema>;

interface Props {
  onSubmit: (data: ExpenseFormData) => void;
}

const ExpenseForm = ({ onSubmit }: Props) => {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<ExpenseFormData>({ resolver: zodResolver(schema) });
  return (
    <form
      onSubmit={handleSubmit((data) => {
        onSubmit(data);
        reset();
      })}
    >
      <div className="mb-3">
        <label htmlFor="description" className="form-label">
          Description
        </label>
        <input
          {...register("description")}
          id="description"
          type="text"
          className="form-control"
        />
        {errors.description && (
          <p className="text-danger">{errors.description.message}</p>
        )}
      </div>
      <br />
      <div className="mb-3">
        <label htmlFor="amount" className="form-label">
          Amount
        </label>
        <input
          {...register("amount", { valueAsNumber: true })}
          id="amount"
          type="number"
          className="form-control"
        />
        {errors.amount && (
          <p className="text-danger">{errors.amount.message}</p>
        )}
      </div>
      <br />
      <div className="mb-3">
        <label htmlFor="category" className="form-label">
          Category
        </label>
        <select {...register("category")} id="category" className="form-select">
          <option value=""></option>
          {categories.map((category) => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
        </select>
        {errors.category && (
          <p className="text-danger">{errors.category.message}</p>
        )}
      </div>
      <button className="btn btn-primary">Submit</button>
    </form>
  );
};

export default ExpenseForm;
