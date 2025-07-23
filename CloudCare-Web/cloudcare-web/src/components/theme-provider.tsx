import * as React from "react";

const ThemeContext = React.createContext<{
  isDark: boolean;
  setIsDark: (val: boolean) => void;
}>({ isDark: false, setIsDark: () => {} });

export function ThemeProvider({ children }: { children: React.ReactNode }) {
  const [isDark, setIsDark] = React.useState(() =>
    typeof window !== "undefined" && document.documentElement.classList.contains("dark")
  );

  React.useEffect(() => {
    if (isDark) {
      document.documentElement.classList.add("dark");
    } else {
      document.documentElement.classList.remove("dark");
    }
  }, [isDark]);

  return (
    <ThemeContext.Provider value={{ isDark, setIsDark }}>
      {children}
    </ThemeContext.Provider>
  );
}

export function useTheme() {
  return React.useContext(ThemeContext);
} 