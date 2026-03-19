import Sidebar from './components/Sidebar';

function App() {
  return (
    // Обертка для всей страницы с легким серым фоном
    <div className="flex min-h-screen bg-gray-50">
      
      {/* Наше боковое меню */}
      <Sidebar />

      {/* Основная часть, куда мы будем добавлять контент */}
      {/* ml-64 делает отступ слева, чтобы меню не перекрывало текст */}
      <div className="ml-64 p-8 w-full">
        <h1 className="text-3xl font-bold">Привет, это будущий клон Kaggle!</h1>
      </div>
      
    </div>
  )
}

export default App