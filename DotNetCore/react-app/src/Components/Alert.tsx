//--------------------------------------------
//Passing childern
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915251

// import { ReactNode } from "react";

// interface Props {
//   children: ReactNode;
// }

// const Alert = ({ children }: Props) => {
//   return <div className="alert alert-primary">{children}</div>;
// };

//--------------------------------------------
// Showing & Alert
//https://members.codewithmosh.com/courses/ultimate-react-part1-1/lectures/45915243

import { ReactNode } from "react";

interface Props {
  children: ReactNode;
  onClose: () => void;
}

const Alert = ({ children, onClose }: Props) => {
  return (
    <div className="alert alert-primary alert-dismissible">
      {children}
      <button
        type="button"
        className="btn-close"
        onClick={onClose}
        data-bs-dismiss="alert"
        aria-label="Close"
      ></button>
    </div>
  );
};

export default Alert;
