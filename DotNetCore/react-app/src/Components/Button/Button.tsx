import styles from "./Button.module.css";

interface Props {
  children: string;
  color?: "primary" | "Secondary" | "danger";
  onClick: () => void;
}
function Button({ children, onClick, color = "primary" }: Props) {
  return (
    // <button className={"btn btn-" + color} onClick={onClick}>
    //   {children}
    // </button>

    <button
      className={[styles.btn, styles["btn-" + color]].join(" ")}
      onClick={onClick}
    >
      {children}
    </button>
  );
}

export default Button;
