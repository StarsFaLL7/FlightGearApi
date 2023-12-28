import { configureStore } from '@reduxjs/toolkit'
import chartReducer from './ChartSlice/ChartSlice'

export const store = configureStore({
  reducer: {
    chart: chartReducer,
  },
})