import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import Analyze from './pages/Analyze/Analyze';
import Plan from './pages/Plan/Plan';
import { store } from './redux/store'
import { Provider } from 'react-redux'
import { createBrowserRouter, RouterProvider } from "react-router-dom";

const router = createBrowserRouter([
  {
    path: "/analyze",
    element: <Analyze />,
  },
  {
    path: "/",
    element: <Plan />,
  },
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <Provider store={store}>
    <React.StrictMode>
      <RouterProvider router={router} />
    </React.StrictMode>
  </Provider>
);

