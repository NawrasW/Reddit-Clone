/** @type {import('tailwindcss').Config} */
const colors = require('tailwindcss/colors')
module.exports = {
  purge: {
    enabled: true,
    content: ['./src/**/*.{html,ts}']
  },
  theme: {
    fontFamily: {
      'smooth': 'Nunito, system-ui, -apple-system, BlinkMacSystemFont, Segoe UI, Roboto, Helvetica Neue, Arial, Noto Sans, sans-serif, Apple Color Emoji, Segoe UI Emoji, Segoe UI Symbol, Noto Color Emoji',
      'sans': '"Montserrat", ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, "Noto Sans", sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol", "Noto Color Emoji"',
    },
    extend: {
      borderRadius: {
        'btn': '4px',
      },
      colors: {
        transparent: 'transparent',
        gray: colors.gray,
        red: colors.red,
        orange: colors.orange,
        yellow: colors.amber,
        lime: colors.lime,
        green: colors.emerald,
        cyan: colors.cyan,
        blue: colors.blue,
        indigo: colors.indigo,
        purple: colors.purple,
        pink: colors.pink,
        black: colors.black,
        white: colors.white,
        custom: {
          'primary': '#323743',
          'secondary': '#272b34',
          'third': '#1d2228'
        },
      },
      zIndex: {
        '100': '100',
      }
    },
  },
  plugins: [],
  safelist: [
    'bg-amber-700',
    'bg-red-500',
    'bg-orange-500',

  ],
}
