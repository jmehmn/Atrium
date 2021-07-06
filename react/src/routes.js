import React from "react";

import Login from "./components/login/Login";
import HomePage from "./components/homePage/HomePage";

const routes = {
  "/": () => <Login />,
  "/home": () => <HomePage />,
};

export default routes;
