import { PropsWithChildren } from "react";
import Header from "./components/Header";

function Layout({ children }: PropsWithChildren) {
  return (
    <>
      <div className="">
      <main>{children}</main>
      </div>
    </>
  );
}

export default Layout;
